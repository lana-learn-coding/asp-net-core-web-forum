<template>
  <crud-table
    :table="table"
    :form="form"
    title="Categories"
    url="/categories"
    :filter="filter"
  >
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
      <v-spacer class="d-none d-md-block"></v-spacer>
    </template>

    <template #form.icon="{value, input, error}">
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

    <template #form.name="{value, input, error}">
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

    <template #table.icon="{ item }">
      <v-icon>{{ item.icon || 'folder' }}</v-icon>
    </template>

    <template #table.createdAt="{ item }">
      {{ formatDate(item.createdAt) }}
    </template>

    <template #table.updatedAt="{ item }">
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
