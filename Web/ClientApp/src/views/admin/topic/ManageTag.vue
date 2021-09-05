<template>
  <crud-table
    :table="table"
    title="Tags"
    url="tags"
    :filter="filter"
    :form="form"
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

    <template #form="{ values, inputs, errors}">
      <v-text-field
        :value="values.name"
        :error-messages="errors.name"
        @input="inputs.name"
        label="Name"
        persistent-placeholder
        required
      >
      </v-text-field>
      <v-text-field
        :value="values.color"
        :error-messages="errors.color"
        @input="inputs.color"
        label="Color"
        persistent-placeholder
      >
      </v-text-field>
    </template>

    <template #table.color="{ item }">
      <v-chip label small :color="item.color"></v-chip>
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
  name: 'ManageTag',
  components: { CrudEditForm, CrudTable },
  setup() {
    const table = [
      {
        text: 'Id',
        sortable: false,
        value: 'slug',
      },
      { text: 'Color', value: 'color' },
      { text: 'Name', value: 'name' },
      { text: 'Created At', value: 'createdAt' },
      { text: 'Action', value: 'action' },
    ];

    const filter = reactive({
      search: '',
    });

    const form = reactive({
      name: '',
      color: '',
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
