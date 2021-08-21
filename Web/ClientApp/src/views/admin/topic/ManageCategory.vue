<template>
  <crud-table :table="table" title="Categories" url="/categories" :filter="filter">
    <template #filter="{bind, on}">
      <v-text-field
        v-debounce="on.search"
        :value="bind.search"
        append-icon="search"
        label="Search"
        single-line
        hide-details
      >
      </v-text-field>
      <v-spacer></v-spacer>
      <div></div>
    </template>

    <template #form="{slug, on}">
      <crud-edit-form
        v-on="on"
        :value="slug"
        :form="form"
        title="Category"
        url="categories"
      >
        <template #field.icon="{value, input, error}">
          <v-text-field
            :value="value"
            :error-messages="error"
            @input="input"
            label="Icon"
            persistent-placeholder
            required
          >
          </v-text-field>
        </template>

        <template #field.name="{value, input, error}">
          <v-text-field
            :value="value"
            :error-messages="error"
            @input="input"
            label="Name"
            persistent-placeholder
            required
          >
          </v-text-field>
        </template>
      </crud-edit-form>
    </template>

    <template #item.icon="{ item }">
      <v-icon>{{ item.icon || 'folder' }}</v-icon>
    </template>

    <template #item.createdAt="{ item }">
      {{ formatDate(item.createdAt) }}
    </template>

    <template #item.updatedAt="{ item }">
      {{ formatDate(item.updatedAt) }}
    </template>
  </crud-table>
</template>

<script lang="ts">
import { defineComponent, reactive } from '@vue/composition-api';
import { formatDate } from '@/composable/date';
import CrudTable from '@/components/CrudTable.vue';
import CrudEditForm from '@/components/CrudEditForm.vue';

export default defineComponent({
  name: 'ManageCategory',
  components: { CrudEditForm, CrudTable },
  setup() {
    const table = [
      {
        text: 'Id',
        sortable: false,
        value: 'slug',
      },
      { text: 'icon', value: 'icon' },
      { text: 'Name', value: 'name' },
      { text: 'Created At', value: 'createdAt' },
      { text: 'Action', value: 'action' },
    ];

    const filter = reactive({
      search: '',
    });

    const form = reactive({
      icon: '',
      name: '',
    });

    return {
      table,
      filter,
      formatDate,
      form,
    };
  },
});
</script>
