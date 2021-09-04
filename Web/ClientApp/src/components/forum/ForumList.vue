<template>
  <v-card flat>
    <v-hover #default="{ hover }">
      <div
        @click="$router.push({ name: 'Forums', query: { category: category.slug } })"
        style="cursor: pointer"
      >
        <v-card-title :class="{ 'text--secondary': hover }">{{ category.name }}</v-card-title>
        <v-card-subtitle v-if="category.description">{{ category.description }}</v-card-subtitle>
      </div>
    </v-hover>

    <v-card-text class="pt-0">
      <slot name="action"></slot>
      <v-skeleton-loader v-if="loading" type="list-item-avatar-three-line@5">
      </v-skeleton-loader>
      <div v-else-if="!forums.length">
        <v-divider></v-divider>
        <div class="body-1 text-center pa-8">Nothing here :(</div>
      </div>
      <div v-else v-for="forum in forums" :key="forum.uid">
        <v-divider></v-divider>
        <forum-row :forum="forum"></forum-row>
      </div>
      <slot name="footer"></slot>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { defineComponent, PropType } from '@vue/composition-api';
import { Category, Forum } from '@/services/model';
import ForumRow from '@/components/forum/ForumRow.vue';

export default defineComponent({
  name: 'ForumList',
  components: { ForumRow },
  props: {
    category: {
      type: Object as PropType<Category>,
    },
    forums: {
      type: Array as PropType<Forum[]>,
      default: () => [],
    },
    loading: {
      type: Boolean,
    },
  },
});
</script>
