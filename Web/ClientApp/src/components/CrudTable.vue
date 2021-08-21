<template>
  <v-card class="elevation-2">
    <v-card-title>
      {{ title }}
      <v-spacer></v-spacer>
      <slot name="action" :bind="query" :on="onQuery"></slot>
      <v-dialog v-model="dialog" width="unset">
        <template #activator="{on, attrs}">
          <v-btn color="primary" v-bind="attrs" v-on="on">Create</v-btn>
        </template>
        <slot name="form" :slug="slug" :on="onForm"></slot>
      </v-dialog>
    </v-card-title>
    <v-card-subtitle class="d-flex">
      <slot name="filter" :bind="query" :on="onQuery"></slot>
    </v-card-subtitle>
    <v-data-table
      :headers="table"
      :items="data"
      :server-items-length="meta.totalItems"
      :items-per-page="query.size"
      :page="query.page"
      :loading="meta.loading"
    >
      <!-- Table column slot-->
      <template v-for="(_, col) of table" v-slot:[`item.${col}`]="{ item }">
        <slot :name="`item.${col}`" :item="item">{{ item[col] }}</slot>
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
  </v-card>
</template>

<script lang="ts">
import { defineComponent, PropType } from '@vue/composition-api';
import { DataTableHeader } from 'vuetify';
import { useTitle } from '@vueuse/core';
import { useHttp, useQuery, useRouteQuery } from '@/services/http';
import { Dictionary } from '@/services/model';
import { useSetters } from '@/composable/compat';

export default defineComponent({
  name: 'CrudTable',
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
      type: Object,
      required: false,
    },
    table: {
      type: Array as PropType<DataTableHeader[]>,
      required: true,
    },
  },
  setup(props) {
    useTitle(`Manage ${props.title}`);
    const { query, data, meta, fetch } = useQuery<Dictionary>(props.url)({ ...props.filter });
    const dialog = useRouteQuery<boolean>('_dialog', false);
    const slug = useRouteQuery<string>('_slug');

    const http = useHttp();

    async function remove(id: string) {
      await http.delete(`${props.url}/${id}`);
      data.value = data.value.filter((item) => item.slug !== id);
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
    };
  },
});
</script>
