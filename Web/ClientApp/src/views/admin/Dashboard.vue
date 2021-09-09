<template>
  <div>
    <app-forum-statistics></app-forum-statistics>
    <app-latest-threads></app-latest-threads>
  </div>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import NumberCounter from '@/components/NumberCounter.vue';
import { formatDateTime } from '@/composable/date';
import AppForumStatistics from '@/components/app/AppForumStatistics.vue';
import AppLatestThreads from '@/components/app/AppLatestThreads.vue';

export default defineComponent({
  name: 'Dashboard',
  components: { AppLatestThreads, AppForumStatistics, NumberCounter },
  setup() {
    useTitle('DashBoard');

    return {
      formatDateTime,
    };
  },
});
</script>
