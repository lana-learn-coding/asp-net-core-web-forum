<template>
  <v-card flat>
    <template v-if="!thread.uid">
      <v-card-text class="pb-0">
        <v-skeleton-loader type="heading" class="mb-2"></v-skeleton-loader>
        <v-skeleton-loader type="paragraph"></v-skeleton-loader>
      </v-card-text>
    </template>
    <template v-else>
      <v-card-title>{{ thread.title }}</v-card-title>
      <v-card-subtitle>
        <div class="d-flex">
          <div class="d-inline mr-2">
            <v-icon class="material-icons-outlined mr-1" small>person</v-icon>
            <span>{{ thread.user.username }}</span>
          </div>
          <div class="d-inline mr-2">
            <v-icon class="material-icons-outlined mr-1" small>schedule</v-icon>
            <span class="d-none d-md-inline">{{ formatDateTime(thread.createdAt) }}</span>
            <span class="d-md-none">{{ formatDate(thread.createdAt) }}</span>
          </div>
          <v-spacer class="d-none d-md-block"></v-spacer>
          <div class="d-inline mr-2">
            <v-icon class="material-icons-outlined mr-1" small>visibility</v-icon>
            <span>{{ thread.viewsCount || thread.postsCount * 2 }}</span>
          </div>
          <div class="d-inline mr-2">
            <v-icon class="material-icons-outlined mr-1" small>chat_bubble_outline</v-icon>
            <span>{{ thread.postsCount }}</span>
          </div>
        </div>
        <router-link
          :to="{ name: 'Forum', params: { slug: thread.forum.slug }}"
          class="text-decoration-none"
        >
          In: {{ thread.forum.title }}
        </router-link>
        <div class="mt-2" v-if="thread.tags.length">
          Tags:
          <v-chip
            v-for="tag of thread.tags"
            class="mr-1"
            :key="tag.uid"
            :color="tag.color"
            label
            outlined
            small
          >
            {{ tag.name }}
          </v-chip>
        </div>
        <div class="mt-2" v-if="thread.status !== 0">
          This thread was closed
        </div>
      </v-card-subtitle>
    </template>

    <v-card-text>
      <slot name="action"></slot>
      <v-skeleton-loader v-if="!thread.uid" type="list-item-avatar-three-line"></v-skeleton-loader>
      <template v-else>
        <post-row :post="thread" class="mt-3"></post-row>
        <v-divider class="mb-3 grey"></v-divider>
      </template>

      <v-skeleton-loader
        v-if="loading || !thread.uid"
        type="list-item-avatar-three-line@3"
      >
      </v-skeleton-loader>

      <div v-else-if="!posts.length">
        <div class="body-1 text-center pa-8">Empty :(</div>
      </div>
      <template v-else>
        <div v-for="post in posts" :key="post.uid">
          <post-row :post="post"></post-row>
          <v-divider class="mb-3"></v-divider>
        </div>
      </template>
      <slot name="footer"></slot>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { defineComponent, PropType } from '@vue/composition-api';
import { Post, Thread } from '@/services/model';
import { formatDate, formatDateTime } from '@/composable/date';
import PostRow from '@/components/forum/PostRow.vue';

export default defineComponent({
  name: 'PostList',
  components: { PostRow },
  props: {
    thread: {
      type: Object as PropType<Thread>,
    },
    posts: {
      type: Array as PropType<Post[]>,
      default: () => [],
    },
    loading: {
      type: Boolean,
    },
  },
  setup() {
    return {
      formatDateTime,
      formatDate,
    };
  },
});
</script>
