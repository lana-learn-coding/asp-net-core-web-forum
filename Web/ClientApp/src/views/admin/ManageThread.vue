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
    </template>

    <template #form>
    </template>

    <template #table.user="{ item }">
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
      <div style="max-width: 350px" class="text-truncate">
        {{ item.title }}
      </div>
    </template>

    <template #table.forum="{ item }">
      <div style="max-width: 200px" class="text-truncate">
        {{ item.forum.title }}
      </div>
    </template>

    <template #table.tags="{ item }">
      <div style="max-width: 300px">
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
import CategorySelect from '@/components/form/CategorySelect.vue';
import AccessSelect from '@/components/form/AccessSelect.vue';
import { usePriority, useThreadStatus } from '@/composable/form';
import EditorInput from '@/components/form/EditorInput.vue';
import { useUser } from '@/services/auth';
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
      { text: 'User', value: 'user' },
      { text: 'Forum', value: 'forum' },
      { text: 'Title', value: 'title' },
      { text: 'Tags', value: 'tags' },
      { text: 'Posts', value: 'postsCount' },
      { text: 'Vote', value: 'vote' },
      { text: 'Status', value: 'status' },
      { text: 'Updated At', value: 'updatedAt' },
      { text: 'Action', value: 'action', sortable: false },
    ];

    const filter = reactive({
      search: '',
      status: '',
      forum: '',
    });

    const { user } = useUser();
    const form = reactive({
      title: '',
      content: '',
      forumId: '',
      tagIds: [],
      userId: user.uid,
      status: 0,
    });

    return {
      table,
      filter,
      formatDate,
      form,
      status: useThreadStatus(),
      getPriority: usePriority,
    };
  },
});
</script>
