<template>
  <v-row class="py-2 py-xl-4">
    <v-col cols="2" xl="1" class="d-flex flex-column align-center justify-center">
      <v-avatar>
        <v-img
          :src="thread.user.avatar || '@/assets/anon.png'"
          lazy-src="@/assets/anon_thumbnail.png"
          :alt="thread.user.username"
        ></v-img>
      </v-avatar>
      <div style="max-width: 100%">
        <div class="text-truncate caption font-weight-medium">
          {{ thread.user.username }}
        </div>
      </div>
    </v-col>
    <v-col cols="10" lg="8">
      <router-link
        class="body-1 text-decoration-none font-weight-medium text-truncate d-block"
        :class="{ 'text--secondary': thread.status !== 0 }"
        :to="{ name: 'Thread', params: { slug: thread.slug } }"
      >
        {{ thread.title }}
      </router-link>
      <div>
        <span class="d-none d-md-inline mr-1">Last activity</span>
        {{ formatDateTime(thread.lastActivityAt) }}
      </div>
      <div class="d-lg-none">
        <div class="mr-1 d-inline">
          <v-icon small>chat_bubble_outline</v-icon>
          {{ thread.postsCount }}
        </div>

        <div class="mr-1 d-inline">
          <v-icon class="material-icons-outlined" small>visibility</v-icon>
          {{ thread.viewsCount || thread.postsCount * 2 }}
        </div>

        <div class="mr-1 d-inline">
          <v-icon class="material-icons-outlined" small>medication</v-icon>
          {{ thread.vote }}
        </div>
      </div>
      <div>
        <v-chip
          v-for="tag of thread.tags"
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
    </v-col>
    <v-col class="d-none d-lg-block" cols="2">
      <div class="body-2 d-md-block mr-3">
        <number-counter
          from="0"
          :to="thread.postsCount"
          :duration="1"
        >
        </number-counter>
        Posts
      </div>
      <div class="body-2 d-md-block mr-3">
        <number-counter
          from="0"
          :to="thread.viewsCount || thread.postsCount * 2"
          :duration="1"
        >
        </number-counter>
        Views
      </div>
      <div class="body-2 d-md-block mr-3">
        <number-counter
          from="0"
          :to="thread.vote"
          :duration="1"
        >
        </number-counter>
        Vote
      </div>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { defineComponent, PropType } from '@vue/composition-api';
import { formatDateTime } from '@/composable/date';
import { Thread } from '@/services/model';
import NumberCounter from '@/components/NumberCounter.vue';

export default defineComponent({
  name: 'ThreadRow',
  components: { NumberCounter },
  props: {
    thread: {
      required: true,
      type: Object as PropType<Thread>,
    },
  },
  setup() {
    return {
      formatDateTime,
    };
  },
});
</script>
