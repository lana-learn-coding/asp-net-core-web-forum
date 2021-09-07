<template>
  <crud-table
    :table="table"
    :form="form"
    title="Threads"
    url="admin/threads"
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
        <auto-complete-select
          class="mr-3"
          label="Forums"
          uri="forums/all"
          style="max-width: 400px"
          item-text="title"
          :value="bind.forum"
          @input="on.forum"
          item-value="slug"
          single-line
          hide-details
        >
        </auto-complete-select>
        <v-spacer class="d-none d-md-block"></v-spacer>
        <thread-status-select
          style="max-width: 200px"
          :value="bind.status"
          @input="on.status"
          single-line
          hide-details
        >
        </thread-status-select>
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
          <auto-complete-select
            label="Forums"
            uri="forums/all"
            :value="values.forumId"
            @input="inputs.forumId"
            item-text="title"
            item-value="uid"
            persistent-placeholder
            required
          >
          </auto-complete-select>
        </v-col>

        <v-col cols="12">
          <auto-complete-select
            label="Tags"
            uri="tags/all"
            :value="values.tagIds"
            @input="inputs.tagIds"
            :error-messages="errors.tagIds"
            item-text="name"
            item-value="uid"
            multiple
            persistent-placeholder
          >
          </auto-complete-select>
        </v-col>

        <v-col>
          <thread-status-select
            :value="values.status"
            @input="inputs.status"
            :error-messages="errors.status"
            persistent-placeholder
            required
          >
          </thread-status-select>
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
            :value="values.content"
            :error-messages="errors.content"
            @input="inputs.content"
          >
          </editor-input>
        </v-col>
      </v-row>
    </template>

    <template #table.user.username="{ item }">
      <v-tooltip bottom>
        <template v-slot:activator="{ on, attrs }">
          <v-avatar size="30">
            <v-img
              v-on="on"
              v-bind="attrs"
              :src="item.user.avatar || '@/assets/anon_.png'"
              lazy-src="@/assets/anon_thumbnail.png">
            </v-img>
          </v-avatar>
        </template>
        <span>{{ item.user.username }}</span>
      </v-tooltip>
    </template>

    <template #table.title="{ item }">
      <div style="max-width: 250px" class="text-truncate">
        {{ item.title }}
      </div>
    </template>

    <template #table.forum.title="{ item }">
      <div style="max-width: 150px" class="text-truncate">
        {{ item.forum.title }}
      </div>
    </template>

    <template #table.tags="{ item }">
      <div style="max-width: 200px">
        <v-chip
          v-for="tag of item.tags"
          class="mr-1"
          :key="tag.uid"
          :color="tag.color"
          label
          outlined
          x-small
        >
          {{ tag.name }}
        </v-chip>
      </div>
    </template>

    <template #table.status="{ item }">
      <v-chip
        :color="status[item.status].color"
        dark
        label
        small
      >
        {{ status[item.status].name }}
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
import { usePriority, useThreadStatus } from '@/composable/form';
import EditorInput from '@/components/form/EditorInput.vue';
import ThreadStatusSelect from '@/components/form/ThreadStatusSelect.vue';
import ForumSelect from '@/components/form/ForumSelect.vue';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';

export default defineComponent({
  name: 'ManageThread',
  components: {
    AutoCompleteSelect,
    ForumSelect,
    ThreadStatusSelect,
    EditorInput,
    AccessSelect,
    CategorySelect,
    CrudEditForm,
    CrudTable,
  },
  setup() {
    const table = [
      { text: 'User', value: 'user.username' },
      { text: 'Forum', value: 'forum.title' },
      { text: 'Title', value: 'title' },
      { text: 'Tags', value: 'tags', sortable: false },
      { text: 'Posts', value: 'postsCount' },
      { text: 'View', value: 'viewsCount' },
      { text: 'Vote', value: 'vote' },
      { text: 'Status', value: 'status' },
      { text: 'Last Activity', value: 'lastActivityAt' },
      { text: 'Updated At', value: 'updatedAt' },
      { text: 'Action', value: 'action', sortable: false },
    ];

    const filter = reactive({
      search: '',
      status: '',
      forum: '',
    });

    const form = reactive({
      title: '',
      content: '',
      forumId: '',
      tagIds: [],
      priority: 20,
      status: 0,
    });

    return {
      table,
      filter,
      formatDateTime,
      form,
      status: useThreadStatus(),
      getPriority: usePriority,
    };
  },
});
</script>
