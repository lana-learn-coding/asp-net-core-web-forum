<template>
  <crud-table
    :table="table"
    :form="form"
    title="Experiences"
    url="exp/experiences"
    :filter="filter"
    form-width="400px"
  >
    <template #filter="{ bind, on }">
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

    <template #form="{ values, inputs, errors }">
      <v-row>
        <v-col cols="12">
          <v-text-field
            :value="values.measurement"
            :error-messages="errors.measurement"
            @input="inputs.measurement"
            label="Experience"
            persistent-placeholder
            required
          >
          </v-text-field>

          <v-text-field
            :value="values.level"
            :error-messages="errors.level"
            @input="inputs.level"
            label="Level (Order)"
            type="number"
            step="1"
            persistent-placeholder
            required
          >
          </v-text-field>
        </v-col>
      </v-row>
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
  name: 'ManageExperience',
  components: { CrudEditForm, CrudTable },
  setup() {
    const table = [
      {
        text: 'Id',
        sortable: false,
        value: 'slug',
      },
      { text: 'Measurement', value: 'measurement' },
      { text: 'Level', value: 'level' },
      { text: 'Updated At', value: 'updatedAt' },
      { text: 'Action', value: 'action' },
    ];

    const filter = reactive({
      search: '',
    });

    const form = reactive({
      measurement: '',
      level: 0,
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
