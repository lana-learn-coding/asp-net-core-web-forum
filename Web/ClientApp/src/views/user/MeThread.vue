<template>
  <v-col cols="12" md="9">
    <v-card flat>
      <v-card-title>Your Threads</v-card-title>
      <v-card-subtitle class="pb-2">Your created threads across forums</v-card-subtitle>
      <v-card-text>
        <v-divider class="mb-3"></v-divider>
        <div class="d-flex mb-2 mb-lg-3 flex-column flex-md-row">
          <v-text-field
            class="mb-2"
            v-debounce="(x) => query.search = x"
            :value="query.search"
            dense
            append-icon="search"
            label="Search for threads"
            single-line
            hide-details
          >
          </v-text-field>
          <v-spacer class="d-none d-md-block"></v-spacer>
          <auto-complete-select
            class="mb-2"
            label="Forums"
            uri="forums/all"
            item-text="title"
            item-value="slug"
            v-model="query.forum"
            dense
            single-line
            hide-details
          >
          </auto-complete-select>
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
          </div>
        </div>
        <v-skeleton-loader
          v-if="meta.loading"
          type="list-item-avatar-three-line@4"
        >
        </v-skeleton-loader>

        <div v-else-if="!data.length">
          <div class="body-1 text-center pa-8">Nothing here :(</div>
        </div>
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
        <v-pagination
          v-model="query.page"
          :total-visible="$vuetify.breakpoint.mdAndUp ? 8 : 4"
          :length="meta.totalPages"
        >
        </v-pagination>
      </v-card-text>
    </v-card>
  </v-col>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import { Thread } from '@/services/model';
import { useQuery } from '@/services/http';
import { useBreadcrumbs } from '@/composable/breadcrumbs';
import ThreadList from '@/components/forum/ThreadList.vue';
import ThreadSortSelect from '@/components/form/ThreadSortSelect.vue';
import ThreadForm from '@/views/forum/ThreadForm.vue';
import ThreadRow from '@/components/forum/ThreadRow.vue';
import { useRequireAuth } from '@/services/auth';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';

export default defineComponent({
  name: 'MeThread',
  components: { AutoCompleteSelect, ThreadRow, ThreadForm, ThreadSortSelect, ThreadList },
  setup() {
    useTitle('Your Threads');
    useBreadcrumbs([
      { text: 'Home', name: 'Home' },
      { text: 'Me', name: 'Me' },
      { text: 'My Threads', disabled: true },
    ]);

    const { user } = useRequireAuth('User');
    const { query, data, meta } = useQuery<Thread>('threads')({
      search: '',
      sort: '',
      forum: '',
      user: user.slug,
    });

    return {
      query,
      data,
      meta,
    };
  },
});
</script>
