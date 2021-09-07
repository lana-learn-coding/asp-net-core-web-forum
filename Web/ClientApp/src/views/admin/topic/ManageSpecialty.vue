<template>
  <crud-table
    :table="table"
    title="Specialties"
    url="specialties"
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
        readonly
        label="Name"
        persistent-placeholder
        disabled
      >
      </v-text-field>
      <auto-complete-select
        uri="tags/all"
        :error-messages="errors.tagId"
        :value="values.tagId"
        @input="inputs.tagId"
        persistent-placeholder
        label="Assign to a Tag"
        item-value="uid"
        item-text="name"
        required
      >
      </auto-complete-select>
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
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';

export default defineComponent({
  name: 'ManageSpecialty',
  components: { AutoCompleteSelect, CrudEditForm, CrudTable },
  setup() {
    const table = [
      { text: 'Name', value: 'name' },
      { text: 'Updated At', value: 'updatedAt' },
      { text: 'Action', value: 'action' },
    ];

    const filter = reactive({
      search: '',
    });

    const form = reactive({
      name: '',
      tagId: '',
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
