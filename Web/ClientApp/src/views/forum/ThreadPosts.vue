<template>
  <v-row>
    <v-col cols="12" md="9">
      <post-list :thread="thread" :posts="data" :loading="meta.loading">
        <template #action>
          <div class="d-flex mb-2 mb-lg-3 flex-column flex-md-row">
            <v-text-field
              class="mb-2"
              v-debounce="(x) => query.search = x"
              :value="query.search"
              dense
              append-icon="search"
              label="Search for keyword"
              single-line
              hide-details
            >
            </v-text-field>
            <v-spacer class="d-none d-md-block"></v-spacer>
            <div class="d-flex">
              <post-sort-select
                class="mb-2"
                style="max-width: 200px"
                v-model="query.sort"
                label="Sort result"
                dense
                single-line
                hide-details
              >
              </post-sort-select>
              <v-spacer></v-spacer>
              <post-form :thread="thread" class="mb-2 ml-2" @change="fetchLast"></post-form>
            </div>
          </div>
        </template>

        <template #footer>
          <v-pagination
            v-model="query.page"
            :total-visible="$vuetify.breakpoint.mdAndUp ? 8 : 4"
            :length="meta.totalPages"
          ></v-pagination>
        </template>
      </post-list>
    </v-col>

    <v-col cols="12" md="3">
      <app-forum-statistics></app-forum-statistics>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { defineComponent, reactive, watch } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import { Dictionary, Thread } from '@/services/model';
import { useRouter } from '@/composable/compat';
import { useHttp, useQuery } from '@/services/http';
import { useBreadcrumbs } from '@/composable/breadcrumbs';
import AppForumStatistics from '@/components/app/AppForumStatistics.vue';
import { formatDateTime } from '@/composable/date';
import PostList from '@/components/forum/PostList.vue';
import PostSortSelect from '@/components/form/PostSortSelect.vue';
import PostForm from '@/views/forum/PostForm.vue';

export default defineComponent({
  name: 'ThreadPosts',
  components: { PostForm, PostSortSelect, PostList, AppForumStatistics },
  props: {
    slug: {
      required: true,
      type: String,
    },
  },
  setup(props) {
    useTitle('Threads Posts');
    useBreadcrumbs([
      { text: 'Home', name: 'Home' },
      { text: 'Forums', name: 'Home' },
      { text: 'Threads', disabled: true },
      { text: 'Posts', disabled: true },
    ]);

    const thread = reactive<Thread>({ uid: '' } as unknown as Thread);
    const router = useRouter();
    const http = useHttp();

    watch(() => props.slug, async (val) => {
      if (!val) {
        router.push({ name: 'NotFound' });
        return;
      }

      try {
        const res = await http.get<Thread>(`threads/${val}`);
        Object.assign(thread, res);
        query.search = '';
        query.thread = val;
        useTitle(`Thread ${res.title}`);
        useBreadcrumbs([
          { text: 'Home', name: 'Home' },
          { text: 'Forums', name: 'Home' },
          { text: 'Threads', disabled: true },
          { text: res.title, disabled: true },
        ]);
      } catch (e) {
        if (!e.response) {
          router.push({ name: 'NotFound' });
        }
      }
    }, { immediate: true });

    const { query, data, meta, fetch } = useQuery<Dictionary>('posts')({
      search: '',
      sort: '',
      thread: props.slug,
    });

    function fetchLast() {
      query.sort = '';
      query.search = '';
      query.page = meta.totalPages + 1;
      fetch();
    }

    return {
      query,
      data,
      meta,
      thread,
      formatDateTime,
      fetchLast,
    };
  },
});
</script>
