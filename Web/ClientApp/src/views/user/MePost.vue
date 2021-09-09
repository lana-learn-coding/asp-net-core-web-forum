<template>
  <v-col cols="12" md="9">
    <v-card flat>
      <v-card-title>Your Posts</v-card-title>
      <v-card-subtitle class="pb-2">Your reply post across threads</v-card-subtitle>

      <v-card-text>
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
          <auto-complete-select
            class="mb-2"
            label="Thread"
            uri="threads/all"
            item-text="title"
            item-value="slug"
            v-model="query.thread"
            dense
            single-line
            hide-details
          >
          </auto-complete-select>
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
          </div>
        </div>

        <v-skeleton-loader v-if="meta.loading" type="list-item-avatar-three-line@3">
        </v-skeleton-loader>

        <div v-else-if="!data.length">
          <div class="body-1 text-center pa-8">Empty :(</div>
        </div>
        <template v-else>
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
        <v-pagination
          v-model="query.page"
          :total-visible="$vuetify.breakpoint.mdAndUp ? 8 : 4"
          :length="meta.totalPages"
        ></v-pagination>
      </v-card-text>
    </v-card>
  </v-col>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import { Post } from '@/services/model';
import { useQuery } from '@/services/http';
import { useBreadcrumbs } from '@/composable/breadcrumbs';
import PostList from '@/components/forum/PostList.vue';
import PostSortSelect from '@/components/form/PostSortSelect.vue';
import PostForm from '@/views/forum/PostForm.vue';
import { useRequireAuth } from '@/services/auth';
import PostRow from '@/components/forum/PostRow.vue';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';

export default defineComponent({
  name: 'MyPost',
  components: { AutoCompleteSelect, PostRow, PostForm, PostSortSelect, PostList },
  setup() {
    useTitle('Your Threads');
    useBreadcrumbs([
      { text: 'Home', name: 'Home' },
      { text: 'Me', name: 'Me' },
      { text: 'My Threads', disabled: true },
    ]);

    const { user } = useRequireAuth('User');
    const { query, data, meta } = useQuery<Post>('posts')({
      search: '',
      sort: '',
      thread: '',
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
