<template>
  <v-card flat>
    <v-card-title>Latest Threads</v-card-title>
    <v-card-text>
      <v-skeleton-loader v-if="loading" type="paragraph@2, sentences"></v-skeleton-loader>
      <template v-else>
        <v-row v-for="thread of threads" :key="thread.uid">
          <v-col>
            <router-link
              class="body-2 text-decoration-none font-weight-medium text-truncate d-block indigo--text text--accent-1"
              :to="{ name: 'Thread', params: { slug: thread.slug } }"
            >
              {{ thread.title }}
            </router-link>
            <div class="caption font-italic text-truncate">
              <span class="mr-1" v-for="tag of thread.tags" :key="tag.uid">{{ tag.name }},</span>
            </div>
            <div>{{ formatDateTime(thread.lastActivityAt) }}</div>
            <div class="d-flex align-center justify-space-between">
              <span class="font-weight-medium body-2">{{ thread.user.username }}</span>
              <span class="d-md-none d-lg-inline caption">
                <span class="mr-1"><v-icon small>chat_bubble_outline</v-icon>
                  {{ thread.postsCount }}
                </span>

                <span class="mr-1">
                  <v-icon class="material-icons-outlined" small>visibility</v-icon>
                  {{ thread.viewsCount || thread.postsCount * 2 }}
                </span>

                <span class="mr-1">
                  <v-icon class="material-icons-outlined" small>medication</v-icon>
                  {{ thread.vote }}
                </span>
              </span>
            </div>
          </v-col>
        </v-row>
      </template>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref, watch } from '@vue/composition-api';
import NumberCounter from '@/components/NumberCounter.vue';
import { useHttp } from '@/services/http';
import { Thread } from '@/services/model';
import { formatDateTime } from '@/composable/date';
import { useRoute } from '@/composable/compat';

export default defineComponent({
  name: 'AppLatestThreads',
  components: { NumberCounter },
  setup() {
    const http = useHttp();
    const threads = ref<Thread[]>([]);

    const loading = ref(true);

    function fetch() {
      http
        .get<Thread[]>('/tracking/active-threads')
        .then((data) => {
          loading.value = false;
          threads.value = data;
        });
    }

    const route = useRoute();
    watch(() => route.path, fetch);
    onMounted(fetch);

    return {
      threads,
      loading,
      formatDateTime,
    };
  },
});
</script>
