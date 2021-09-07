<template>
  <v-card class="elevation-2">
    <v-card-title>
      {{ title }}
      <v-spacer></v-spacer>
      <slot name="action" :bind="query" :on="onQuery"></slot>
      <v-dialog v-model="dialog" width="unset" transition="fade-transition" eager>
        <template #activator="{on, attrs}">
          <v-btn color="primary" v-bind="attrs" v-on="on">Create</v-btn>
        </template>

        <!-- Form slot. expose #form.<name> slot for customization-->
        <slot :slug="slug" :on="onForm">
          <crud-edit-form
            v-on="onForm"
            :value="slug"
            :form="form || {}"
            :title="title"
            :url="url"
            :width="formWidth"
          >
            <template #form="{values, isEdit, errors, inputs}">
              <slot
                name="form"
                :isEdit="isEdit"
                :values="values"
                :errors="errors"
                :inputs="inputs"
              >
              </slot>
            </template>
          </crud-edit-form>
        </slot>
      </v-dialog>
    </v-card-title>

    <v-card-subtitle class="d-flex">
      <slot name="filter" :bind="query" :on="onQuery"></slot>
    </v-card-subtitle>

    <!-- Data table slot. expose #item.<name> slots (as #table.<name>) for customization-->
    <slot
      name="table"
      :table="table"
      :items="data"
      :meta="meta"
      :query="query"
      :onUpdate="update"
      :onDelete="remove"
      :sortBy="sortBy"
      :sortDesc="sortDesc"
    >
      <v-data-table
        :headers="table"
        :items="data"
        :server-items-length="meta.totalItems"
        :items-per-page.sync="query.size"
        :page.sync="query.page"
        :loading="meta.loading"
        :sort-by.sync="sortBy"
        :sort-desc.sync="sortDesc"
      >
        <template v-for="col of table" v-slot:[`item.${col.value}`]="{ item }">
          <slot :name="`table.${col.value}`" :item="item">{{ item[col.value] }}</slot>
        </template>

        <template #item.action="{ item }">
          <v-btn
            class="mx-2"
            icon
            color="primary"
            @click="update(item.slug)"
          >
            <v-icon>
              edit
            </v-icon>
          </v-btn>

          <v-btn
            class="mx-2"
            icon
            color="error"
            @click="remove(item.slug)"
          >
            <v-icon>
              delete
            </v-icon>
          </v-btn>
        </template>
      </v-data-table>
    </slot>
  </v-card>
</template>

<script lang="ts">
import { computed, defineComponent, PropType } from '@vue/composition-api';
import { DataTableHeader } from 'vuetify';
import { useTitle } from '@vueuse/core';
import { singular } from 'pluralize';
import { useHttp, useQuery } from '@/services/http';
import { Dictionary, FlatDictionary } from '@/services/model';
import { useSetters } from '@/composable/compat';
import CrudEditForm from '@/components/CrudEditForm.vue';
import { useMessage } from '@/composable/message';

export default defineComponent({
  name: 'CrudTable',
  components: { CrudEditForm },
  props: {
    url: {
      type: String,
      required: true,
    },
    title: {
      type: String,
      required: true,
    },
    filter: {
      type: Object as PropType<FlatDictionary>,
      required: false,
    },
    table: {
      type: Array as PropType<DataTableHeader[]>,
      required: true,
    },
    form: {
      type: Object as PropType<Dictionary>,
      required: false,
    },
    formWidth: {
      type: String,
      required: false,
    },
  },
  setup(props) {
    useTitle(`Manage ${props.title}`);
    const { query, data, meta, fetch } = useQuery<Dictionary>(props.url)({
      ...props.filter,
      sort: '',
      _slug: '',
      _dialog: false,
    });
    const dialog = computed({
      get: () => query._dialog ?? false,
      set: (val) => {
        query._dialog = val;
      },
    });
    const slug = computed({
      get: () => query._slug ?? '',
      set: (val) => {
        query._slug = val;
      },
    });

    const http = useHttp();
    const { confirm, notify } = useMessage();

    async function remove(id: string) {
      const ok = await confirm({
        title: 'Confirm delete',
        subtitle: `id: ${id}`,
        type: 'warning',
        text: `You are about to delete ${singular(props.title)}`,
      });
      if (!ok) return;
      await http.delete(`${props.url}/${id}`);
      data.value = data.value.filter((item) => item.slug !== id);
      notify({ text: `${singular(props.title)} deleted` });
    }

    function update(id: string) {
      slug.value = id;
      dialog.value = true;
    }

    const onForm = {
      input: (val: string) => {
        slug.value = val;
      },
      close: () => {
        dialog.value = false;
        slug.value = '';
      },
      change: fetch,
    };

    return {
      slug,
      dialog,
      query,
      data,
      meta,
      update,
      remove,
      fetch,
      onForm,
      onQuery: useSetters(query),
      ...useTableSort(query),
    };
  },
});

function useTableSort(query) {
  const desc = computed<boolean>(
    {
      get() {
        return query.sort?.startsWith('-');
      },
      set(val) {
        if (val && !query.sort.startsWith('-')) {
          query.sort = `-${query.sort}`;
        }

        if (!val && query.sort.startsWith('-')) {
          query.sort = query.sort.substring(1);
        }
      },
    },
  );

  const by = computed<string>({
    get() {
      return query.sort?.startsWith('-') ? query.sort?.substring(1) : query.sort;
    },
    set(val) {
      if (!val) {
        query.sort = '';
        return;
      }
      query.sort = val;
    },
  });

  return {
    sortDesc: desc,
    sortBy: by,
  };
}
</script>
