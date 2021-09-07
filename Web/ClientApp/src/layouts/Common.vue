<template>
  <the-layout>
    <v-row>
      <router-view></router-view>
      <v-col cols="12" md="3">
        <app-forum-statistics></app-forum-statistics>
      </v-col>
    </v-row>
  </the-layout>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';
import TheLayout from '@/layouts/TheLayout.vue';
import AppForumStatistics from '@/components/app/AppForumStatistics.vue';

export default defineComponent({
  name: 'Common',
  components: { AppForumStatistics, TheLayout },
});
</script>
