<template>
  <v-row class="py-2">
    <v-col cols="2" xl="1" class="d-flex flex-column align-center justify-center">
      <v-avatar>
        <v-img
          :src="post.user.avatar || '@/assets/anon.png'"
          lazy-src="@/assets/anon_thumbnail.png"
          :alt="post.user.username"
        ></v-img>
      </v-avatar>
      <div style="max-width: 100%">
        <div class="text-truncate caption font-weight-medium">
          {{ post.user.username }}
        </div>
      </div>
    </v-col>
    <v-col cols="10" xl="11" class="pt-0">
      <div class="d-flex">
        <div class="d-flex align-center mr-2 caption">
          <v-icon class="material-icons-outlined mr-1" x-small>schedule</v-icon>
          <span>{{ formatDateTime(post.createdAt) }}</span>
        </div>
        <v-spacer></v-spacer>
        <div class="d-flex align-center caption" v-if="post.postsCount">
          <v-icon small class="mr-1">star_outline</v-icon>
          <span>Origin</span>
        </div>
      </div>
      <div class="body-1" v-html="post.content"></div>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { defineComponent, PropType } from '@vue/composition-api';
import { formatDateTime } from '@/composable/date';
import { Post } from '@/services/model';
import NumberCounter from '@/components/NumberCounter.vue';

export default defineComponent({
  name: 'PostRow',
  components: { NumberCounter },
  props: {
    post: {
      required: true,
      type: Object as PropType<Post>,
    },
  },
  setup() {
    return {
      formatDateTime,
    };
  },
});
</script>
