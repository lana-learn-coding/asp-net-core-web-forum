<template>
  <crud-table
    :table="table"
    :form="form"
    title="Forums"
    url="admin/forums"
    :filter="filter"
    form-width="900px"
  >
    <template #filter="{bind, on}">
      <div class="d-flex flex-column flex-grow-1 flex-md-row">
        <v-text-field
          class="mr-3"
          v-debounce="on.search"
          :value="bind.search"
          append-icon="search"
          label="Search"
          single-line
          hide-details
        >
        </v-text-field>
        <v-spacer class="d-none d-md-block"></v-spacer>
        <category-select
          :value="bind.category"
          @input="on.category"
          item-value="slug"
          single-line
          hide-details
        >
        </category-select>
        <v-spacer class="d-none d-md-block"></v-spacer>
        <div class="d-flex">
          <access-select
            :value="bind.forumAccess"
            @input="on.forumAccess"
            label="Forum Access"
            class="mr-3"
            single-line
            hide-details
          >
          </access-select>
          <access-select
            :value="bind.threadAccess"
            @input="on.threadAccess"
            label="Thread Access"
            single-line
            hide-details
          >
          </access-select>
        </div>
      </div>
    </template>

    <template #form="{values, inputs, errors}">
      <v-row>
        <v-col cols="12">
          <v-text-field
            :value="values.title"
            :error-messages="errors.title"
            @input="inputs.title"
            label="Title"
            persistent-placeholder
            required
          >
          </v-text-field>
        </v-col>

        <v-col cols="12">
          <v-text-field
            :value="values.subTitle"
            :error-messages="errors.subTitle"
            @input="inputs.subTitle"
            label="Sub Title"
            persistent-placeholder
            required
          >
          </v-text-field>
        </v-col>

        <v-col>
          <access-select
            :value="values.forumAccess"
            @input="inputs.forumAccess"
            :error-messages="errors.forumAccess"
            label="Forum Access"
            persistent-placeholder
            required
          >
          </access-select>
        </v-col>

        <v-col>
          <access-select
            :value="values.threadAccess"
            @input="inputs.threadAccess"
            :error-messages="errors.threadAccess"
            label="Thread Access"
            persistent-placeholder
            required
          >
          </access-select>
        </v-col>

        <v-col cols="12">
          <category-select
            :value="values.categoryId"
            @input="inputs.categoryId"
            :error-messages="errors.categoryId"
            persistent-placeholder
            required
          >
          </category-select>
        </v-col>

        <v-col>
          <v-text-field
            :value="values.icon"
            :error-messages="errors.icon"
            @input="inputs.icon"
            label="Icon"
            hint="Material icon name"
            persistent-placeholder
            required
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
          <editor-input
            :value="values.description"
            :error-messages="errors.description"
            @input="inputs.description"
          >
          </editor-input>
        </v-col>
      </v-row>
    </template>

    <template #table.title="{ item }">
      <div style="max-width: 200px" class="text-truncate">
        {{ item.title }}
      </div>
    </template>

    <template #table.category.name="{ item }">
      <div style="max-width: 150px" class="text-truncate">
        {{ item.category.name }}
      </div>
    </template>

    <template #table.forumAccess="{ item }">
      <v-chip
        :color="access[item.forumAccess].color"
        label
        small
        dark
      >
        {{ access[item.forumAccess].name }}
      </v-chip>
      /
      <v-chip
        :color="access[item.threadAccess].color"
        dark
        label
        small
      >
        {{ access[item.threadAccess].name }}
      </v-chip>
    </template>

    <template #table.priority="{ item }">
      <v-chip :color="getPriority(item.priority).color" dark label small>
        {{ getPriority(item.priority).name }} : {{ item.priority }}
      </v-chip>
    </template>

    <template #table.lastActivityAt="{ item }">
      {{ formatDateTime(item.lastActivityAt) }}
    </template>

    <template #table.updatedAt="{ item }">
      {{ formatDateTime(item.updatedAt) }}
    </template>
  </crud-table>
</template>

<script lang="ts">
import { defineComponent, reactive } from '@vue/composition-api';
import { formatDateTime } from '@/composable/date';
import CrudTable from '@/components/CrudTable.vue';
import CrudEditForm from '@/components/CrudEditForm.vue';
import CategorySelect from '@/components/form/CategorySelect.vue';
import AccessSelect from '@/components/form/AccessSelect.vue';
import { useAccessType, usePriority } from '@/composable/form';
import EditorInput from '@/components/form/EditorInput.vue';

export default defineComponent({
  name: 'ManageForum',
  components: { EditorInput, AccessSelect, CategorySelect, CrudEditForm, CrudTable },
  setup() {
    const table = [
      { text: 'Title', value: 'title' },
      { text: 'Category', value: 'category.name' },
      { text: 'Priority', value: 'priority' },
      { text: 'Forum/Thread Access', value: 'forumAccess' },
      { text: 'Threads', value: 'threadsCount' },
      { text: 'Posts', value: 'postsCount' },
      { text: 'Views', value: 'viewsCount' },
      { text: 'Last activity', value: 'lastActivityAt' },
      { text: 'Updated At', value: 'updatedAt' },
      { text: 'Action', value: 'action', sortable: false },
    ];

    const filter = reactive({
      search: '',
      category: '',
      forumAccess: '',
      threadAccess: '',
    });

    const form = reactive({
      title: '',
      categoryId: '',
      threadAccess: 0,
      forumAccess: 0,
      subTitle: '',
      icon: '',
      description: '',
      priority: 20,
    });

    return {
      table,
      filter,
      formatDateTime,
      form,
      access: useAccessType(),
      getPriority: usePriority,
    };
  },
});
</script>
