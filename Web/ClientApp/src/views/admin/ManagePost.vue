<template>
  <crud-table
    :table="table"
    :form="form"
    title="Posts"
    url="posts"
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
        label="Thread"
        uri="threads/all"
        style="max-width: 400px"
        item-text="title"
        :value="bind.thread"
        @input="on.thread"
        item-value="slug"
        single-line
        hide-details
      >
      </auto-complete-select>
    </template>

    <template #form="{values, errors, inputs}">
      <v-row>
        <v-col cols="12">
          <auto-complete-select
            label="Thread"
            uri="threads/all"
            :value="values.threadId"
            @input="inputs.threadId"
            item-text="title"
            item-value="uid"
            persistent-placeholder
            required
          >
          </auto-complete-select>
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

    <template #table.thread="{ item }">
      <div style="max-width: 200px" class="text-truncate">
        {{ item.threadTitle }}
      </div>
    </template>

    <template #table.content="{ item }">
      <div style="max-width: 500px" class="text-truncate" v-html="item.content"></div>
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
import EditorInput from '@/components/form/EditorInput.vue';
import ThreadStatusSelect from '@/components/form/ThreadStatusSelect.vue';
import ForumSelect from '@/components/form/ForumSelect.vue';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';

export default defineComponent({
  name: 'ManagePost',
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
      { text: 'Thread', value: 'thread' },
      { text: 'Content', value: 'content' },
      { text: 'Vote', value: 'vote' },
      { text: 'Updated At', value: 'updatedAt' },
      { text: 'Action', value: 'action', sortable: false },
    ];

    const filter = reactive({
      search: '',
      thread: '',
    });

    const form = reactive({
      threadId: '',
      content: '',
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
