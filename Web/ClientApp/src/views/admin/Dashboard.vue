<template>
  <div>
    <v-card flat class="mb-4">
      <v-card-title>Statistics</v-card-title>
      <v-card-text>
        <v-skeleton-loader type="text@6" v-if="loading.statistics"></v-skeleton-loader>
        <template v-else>
          <div class="mb-2">
            <b>
              <v-icon small class="mr-1">question_answer</v-icon>
              <number-counter from="0" :to="statistics.totalPosts"></number-counter>
            </b> Total Posts
          </div>
          <div class="mb-2">
            <b>
              <v-icon small class="mr-1">chat_bubble</v-icon>
              <number-counter from="0" :to="statistics.totalThreads"></number-counter>
            </b> Total Threads
          </div>
          <div class="mb-2">
            <b>
              <v-icon small class="mr-1">people</v-icon>
              <number-counter from="0" :to="statistics.totalMembers"></number-counter>
            </b> Members
          </div>
          <div class="mb-2">
            <b>
              <v-icon small class="mr-1">record_voice_over</v-icon>
              <number-counter from="0" :to="statistics.onlineMembers"></number-counter>
            </b> Online
          </div>
          <div>
            <b>
              <v-icon small class="mr-1">fiber_manual_record</v-icon>
              <number-counter from="0" :to="statistics.onlineAnonymous"></number-counter>
            </b> Anonymous
          </div>
        </template>
      </v-card-text>
    </v-card>

    <v-card flat class="mb-4">
      <v-card-title>Latest Threads</v-card-title>
      <v-card-text>
        <v-skeleton-loader v-if="loading.threads" type="paragraph@2, sentences"></v-skeleton-loader>
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
              </span>
              </div>
            </v-col>
          </v-row>
        </template>
      </v-card-text>
    </v-card>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, reactive, ref } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import NumberCounter from '@/components/NumberCounter.vue';
import { useHttp } from '@/services/http';
import { Thread } from '@/services/model';
import { formatDateTime } from '@/composable/date';

export default defineComponent({
  name: 'AppForumStatistics',
  components: { NumberCounter },
  setup() {
    useTitle('DashBoard');
    const http = useHttp();
    const statistics = reactive({
      totalMembers: 0,
      totalThreads: 0,
      totalPosts: 0,
      onlineMembers: 0,
      onlineAnonymous: 0,
    });

    const threads = ref<Thread[]>([]);

    const loading = reactive({
      statistics: true,
      threads: true,
    });

    onMounted(() => {
      http
        .get('/tracking/statistics')
        .then((val) => {
          Object.assign(statistics, val);
          loading.statistics = false;
        });

      http
        .get<Thread[]>('/tracking/active-threads')
        .then((data) => {
          loading.threads = false;
          threads.value = data;
        });
    });

    return {
      threads,
      statistics,
      loading,
      formatDateTime,
    };
  },
});
</script>
