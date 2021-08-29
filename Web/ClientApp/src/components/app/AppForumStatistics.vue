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
        <v-skeleton-loader v-else type="paragraph@2"></v-skeleton-loader>
      </v-card-text>
    </v-card>
    <v-card flat>
      <v-card-title>About us</v-card-title>
      <v-card-text>
        Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum
        has
        been the industry's standard dummy text ever since the 1500s, when an unknown printer
        took
        a galley of type and scrambled it to make a type specimen book. It has survived not only
        five centuries, but also the leap into electronic typesetting, remaining essentially
      </v-card-text>
    </v-card>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, reactive } from '@vue/composition-api';
import NumberCounter from '@/components/NumberCounter.vue';
import { useHttp } from '@/services/http';

export default defineComponent({
  name: 'AppForumStatistics',
  components: { NumberCounter },
  setup() {
    const http = useHttp();
    const statistics = reactive({
      totalMembers: 0,
      totalThreads: 0,
      totalPosts: 0,
      onlineMembers: 0,
      onlineAnonymous: 0,
    });

    const loading = reactive({
      statistics: true,
      threads: true,
    });

    onMounted(() => {
      http
        .get('/tracking/statistics')
        .then((val) => Object.assign(statistics, val))
        .finally(() => {
          loading.statistics = false;
        });

      http
        .get('/tracking/active-threads')
        .finally(() => {
          loading.threads = false;
        });
    });

    return {
      statistics,
      loading,
    };
  },
});
</script>
