<template>
  <the-layout>
    <v-container class="mt-2 mt-md-3 mt-lg-4 mt-xl-5">
      <app-breadcrumbs class="mb-2 mb-md-3 mb-lg-4"></app-breadcrumbs>
      <v-row>
        <router-view></router-view>
        <v-col cols="12" md="3">
          <app-forum-statistics></app-forum-statistics>
        </v-col>
      </v-row>
    </v-container>
  </the-layout>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';
import TheLayout from '@/layouts/TheLayout.vue';
import AppForumStatistics from '@/components/app/AppForumStatistics.vue';
import AppBreadcrumbs from '@/components/app/AppBreadcrumbs.vue';

export default defineComponent({
  name: 'Common',
  components: { AppBreadcrumbs, AppForumStatistics, TheLayout },
});
</script>
