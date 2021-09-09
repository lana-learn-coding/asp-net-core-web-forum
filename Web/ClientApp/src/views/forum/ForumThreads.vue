<template>
  <v-col cols="12" md="9">
    <thread-list :forum="forum" :loading="meta.loading" :threads="data">
      <template #action>
        <div class="d-flex mb-2 mb-lg-3 flex-column flex-md-row">
          <v-text-field
            class="mb-2"
            v-debounce="(x) => query.search = x"
            :value="query.search"
            dense
            append-icon="search"
            label="Search for threads in this forum"
            single-line
            hide-details
          >
          </v-text-field>
          <v-spacer class="d-none d-md-block"></v-spacer>
          <div class="d-flex">
            <thread-sort-select
              class="mb-2"
              style="max-width: 200px"
              v-model="query.sort"
              label="Sort result"
              dense
              single-line
              hide-details
            >
            </thread-sort-select>
            <v-spacer></v-spacer>
            <thread-form :forum="forum" class="mb-2 ml-2" @change="fetchLast"></thread-form>
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
    </thread-list>
  </v-col>
</template>

<script lang="ts">
import { defineComponent, nextTick, reactive, watch } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import { Dictionary, Forum } from '@/services/model';
import { useRouter } from '@/composable/compat';
import { useHttp, useQuery } from '@/services/http';
import { useBreadcrumbs } from '@/composable/breadcrumbs';
import { formatDateTime } from '@/composable/date';
import ThreadList from '@/components/forum/ThreadList.vue';
import ThreadSortSelect from '@/components/form/ThreadSortSelect.vue';
import ThreadForm from '@/views/forum/ThreadForm.vue';

export default defineComponent({
  name: 'ForumThreads',
  components: { ThreadForm, ThreadSortSelect, ThreadList },
  props: {
    slug: {
      required: true,
      type: String,
    },
  },
  setup(props) {
    useTitle('Forums Threads');
    useBreadcrumbs([
      { text: 'Home', name: 'Home' },
      { text: 'Forums', name: 'Search' },
      { text: 'Forums Threads', disabled: true },
    ]);

    const forum = reactive<Forum>({ uid: '' } as unknown as Forum);
    const router = useRouter();
    const http = useHttp();

    watch(() => props.slug, async (val) => {
      if (!val) {
        router.push({ name: 'NotFound' });
        return;
      }

      try {
        const res = await http.get<Forum>(`forums/${val}`);
        Object.assign(forum, res);
        query.search = '';
        query.forum = val;
        useBreadcrumbs([
          { text: 'Home', name: 'Home' },
          { text: 'Forums', name: 'Search' },
          { text: res.title, disabled: true },
        ]);
      } catch (e) {
        if (!e.response) {
          router.push({ name: 'NotFound' });
        }
      }
    }, { immediate: true });

    const { query, data, meta, fetch } = useQuery<Dictionary>('threads')({
      search: '',
      sort: '',
      forum: props.slug,
    });

    async function fetchLast() {
      await fetch({
        sort: '',
        search: '',
        page: 1,
      }, true);
      nextTick(() => window.scrollTo(0, 50));
    }

    return {
      fetchLast,
      query,
      data,
      meta,
      forum,
      formatDateTime,
    };
  },
});
</script>
