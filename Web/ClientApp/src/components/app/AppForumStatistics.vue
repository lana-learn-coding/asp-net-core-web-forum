<template>
  <v-card flat>
    <v-card-title>Statistics</v-card-title>
    <v-card-text>
      <v-skeleton-loader type="text@6" v-if="loading"></v-skeleton-loader>
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
            <number-counter from="0" :to="statistics.onlineMembers" :duration="1"></number-counter>
          </b> Online
        </div>
        <div>
          <b>
            <v-icon small class="mr-1">fiber_manual_record</v-icon>
            <number-counter from="0" :to="statistics.onlineAnonymous"
                            :duration="1"></number-counter>
          </b> Anonymous
        </div>
      </template>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import {
  defineComponent,
  onBeforeUnmount,
  onMounted,
  reactive,
  ref,
  watch,
} from '@vue/composition-api';
import NumberCounter from '@/components/NumberCounter.vue';
import { useHttp } from '@/services/http';
import { formatDateTime } from '@/composable/date';
import { useRoute } from '@/composable/compat';

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
    const loading = ref(true);

    function fetch() {
      http
        .get('/tracking/statistics')
        .then((val) => {
          Object.assign(statistics, val);
          loading.value = false;
        });
    }

    // update data each x2 interval or route change
    const route = useRoute();
    watch(() => route.path, fetch);
    const schedule = setTimeout(fetch, Number(process.env.VUE_APP_API_TRACKING_REFRESH || '1000') * 2000);
    onBeforeUnmount(() => clearTimeout(schedule));
    onMounted(fetch);

    return {
      statistics,
      loading,
      formatDateTime,
    };
  },
});
</script>
