<template>
  <v-col
    cols="12"
    md="9"
  >
    <v-card flat>
      <v-card-title>Advanced search</v-card-title>
      <v-card-subtitle>Click on filter for more search options</v-card-subtitle>

      <v-card-text class="pt-0">
        <div class="d-flex mb-4 mb-lg-6 flex-column flex-md-row">
          <v-text-field
            class="mb-2"
            v-debounce="(x) => query.search = x"
            :value="query.search"
            dense
            append-icon="search"
            label="Search"
            single-line
            hide-details
          >
          </v-text-field>
          <v-spacer class="d-none d-md-block"></v-spacer>
          <div class="d-flex">
            <search-type-select
              class="mb-2 mr-3"
              style="max-width: 200px"
              v-model="type"
              label="Result type"
              dense
              single-line
              hide-details
            >
            </search-type-select>
            <post-sort-select
              class="mb-2"
              v-if="type === 'post'"
              style="max-width: 200px"
              v-model="query.sort"
              label="Sort result"
              dense
              single-line
              hide-details
            >
            </post-sort-select>
            <thread-sort-select
              class="mb-2"
              v-else
              style="max-width: 200px"
              v-model="query.sort"
              label="Sort result"
              dense
              single-line
              hide-details
            >
            </thread-sort-select>
            <v-spacer></v-spacer>
            <advanced-search-form :filter="query" @change="filter"></advanced-search-form>
          </div>
        </div>
        <v-skeleton-loader v-if="meta.loading" type="list-item-avatar-three-line@4">
        </v-skeleton-loader>
        <div v-else-if="!data.length">
          <v-divider></v-divider>
          <div class="body-1 text-center pa-8">Nothing here :(</div>
        </div>

        <template v-else-if="type === 'post'">
          <div v-for="post in data" :key="post.uid">
            <post-row :post="post">
              <template #title>
                <router-link
                  class="mt-1 text-link text-decoration-none text-truncate d-block"
                  :to="{ name: 'Thread', params: { slug: post.threadSlug } }"
                >
                  Thread: {{ post.threadTitle }}
                </router-link>
              </template>
            </post-row>
            <v-divider class="mb-3"></v-divider>
          </div>
        </template>

        <template v-else>
          <div v-for="thread in data" :key="thread.uid">
            <thread-row :thread="thread">
              <template #footer>
                <router-link
                  class="mt-1 text-link text-decoration-none text-truncate d-block"
                  :to="{ name: 'Forum', params: { slug: thread.forum.slug } }"
                >
                  Forum: {{ thread.forum.title }}
                </router-link>
              </template>
            </thread-row>
          </div>
        </template>

        <slot name="footer"></slot>
      </v-card-text>
    </v-card>
  </v-col>
</template>

<script lang="ts">
import { computed, defineComponent } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import { useBreadcrumbs } from '@/composable/breadcrumbs';
import { useQuery } from '@/services/http';
import { Dictionary } from '@/services/model';
import PostRow from '@/components/forum/PostRow.vue';
import ThreadRow from '@/components/forum/ThreadRow.vue';
import ThreadSortSelect from '@/components/form/ThreadSortSelect.vue';
import PostSortSelect from '@/components/form/PostSortSelect.vue';
import SearchTypeSelect from '@/components/form/SearchTypeSelect.vue';
import AdvancedSearchForm from '@/views/forum/AdvancedSearchForm.vue';

export default defineComponent({
  name: 'AdvancedSearch',
  components: {
    AdvancedSearchForm,
    SearchTypeSelect,
    PostSortSelect,
    ThreadSortSelect,
    ThreadRow,
    PostRow,
  },
  setup() {
    useTitle('Advanced Search');
    useBreadcrumbs([
      { text: 'Home', name: 'Home' },
      { text: 'Forums', name: 'Search' },
      { text: 'Advanced', disabled: true },
    ]);

    const { query, data, meta, fetch } = useQuery<Dictionary>('search')({
      search: '',
      type: '',
      tag: '',
      forum: '',
      thread: '',
      experience: '',
      position: '',
      speciality: '',
      city: '',
      country: '',
      user: '',
      sort: '',
    });

    const type = computed<string>({
      get: () => query.type,
      set: (val) => {
        query.thread = '';
        query.sort = '';
        query.type = val;
      },
    });

    function filter(search) {
      Object.assign(query, search);
    }

    return {
      filter,
      type,
      query,
      data,
      meta,
      fetch,
    };
  },
});
</script>
