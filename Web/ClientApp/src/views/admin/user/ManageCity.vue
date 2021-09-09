<template>
  <crud-table
    :table="table"
    :form="form"
    title="Cities"
    url="regions/cities"
    :filter="filter"
    form-width="400px"
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
      <auto-complete-select
        uri="regions/countries/all"
        :value="bind.countryId"
        @input="on.countryId"
        label="Country"
        item-value="uid"
        item-text="name"
        single-line
        hide-details
      >
      </auto-complete-select>
      <v-spacer class="d-none d-md-block"></v-spacer>
    </template>

    <template #form="{ values, inputs, errors}">
      <v-row>
        <v-col cols="12">
          <v-text-field
            :value="values.name"
            :error-messages="errors.name"
            @input="inputs.name"
            label="Name"
            persistent-placeholder
            required
          >
          </v-text-field>
        </v-col>

        <v-col>
          <auto-complete-select
            uri="regions/countries/all"
            :value="values.countryId"
            :error-messages="errors.countryId"
            @input="inputs.countryId"
            persistent-placeholder
            label="Country"
            item-value="uid"
            item-text="name"
            required
          >
          </auto-complete-select>
        </v-col>
      </v-row>
    </template>

    <template #table.country.name="{ item }">
      {{ item.country.name }}
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
import { usePriority } from '@/composable/form';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';

export default defineComponent({
  name: 'ManageCity',
  components: { AutoCompleteSelect, CrudEditForm, CrudTable },
  setup() {
    const table = [
      {
        text: 'Id',
        sortable: false,
        value: 'slug',
      },
      { text: 'Name', value: 'name' },
      { text: 'Country', value: 'country.name' },
      { text: 'Updated At', value: 'updatedAt' },
      { text: 'Action', value: 'action' },
    ];

    const filter = reactive({
      search: '',
      countryId: '',
    });

    const form = reactive({
      name: '',
      countryId: '',
    });

    return {
      table,
      filter,
      formatDate,
      form,
      getPriority: usePriority,
    };
  },
});
</script>
