<template>
  <crud-table
    @change="fetch"
    :table="table"
    :form="form"
    title="Categories"
    url="categories"
    :filter="filter"
    form-width="500px"
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
          <v-text-field
            :value="values.icon"
            :error-messages="errors.icon"
            @input="inputs.icon"
            label="Icon"
            persistent-placeholder
          >
          </v-text-field>
        </v-col>

        <v-col>
          <v-text-field
            :value="values.priority"
            :error-messages="errors.priority"
            @input="inputs.priority"
            label="Priority"
            persistent-placeholder
            type="number"
            min="0"
            step="1"
            :hint="getPriority(values.priority).name"
          >
          </v-text-field>
        </v-col>

        <v-col cols="12">
          <v-text-field
            :value="values.description"
            :error-messages="errors.description"
            @input="inputs.description"
            label="Description"
            persistent-placeholder
          >
          </v-text-field>
        </v-col>
      </v-row>
    </template>

    <template #table.icon="{ item }">
      <v-icon>{{ item.icon || 'folder' }}</v-icon>
    </template>

    <template #table.description="{ item }">
      <div style="max-width: 500px" class="text-truncate">{{ item.description }}</div>
    </template>

    <template #table.priority="{ item }">
      <v-chip :color="getPriority(item.priority).color" dark label small>
        {{ getPriority(item.priority).name }} : {{ item.priority }}
      </v-chip>
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
import { debounce } from 'vue-debounce';
import { formatDate } from '@/composable/date';
import CrudTable from '@/components/CrudTable.vue';
import CrudEditForm from '@/components/CrudEditForm.vue';
import { useCategories, usePriority } from '@/composable/form';

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
      { text: 'Description', value: 'description' },
      { text: 'Priority', value: 'priority' },
      { text: 'Updated At', value: 'updatedAt' },
      { text: 'Action', value: 'action' },
    ];

    const filter = reactive({
      search: '',
    });

    const form = reactive({
      icon: '',
      name: '',
      priority: 20,
      description: '',
    });

    const { fetch } = useCategories();

    return {
      fetch: debounce(fetch, 1000),
      table,
      filter,
      formatDate,
      form,
      getPriority: usePriority,
    };
  },
});
</script>
