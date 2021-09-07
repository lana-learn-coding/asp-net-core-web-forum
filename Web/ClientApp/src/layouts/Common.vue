<template>
  <the-layout>
    <v-container class="mt-2 mt-md-3 mt-lg-4 mt-xl-5">
      <app-breadcrumbs class="mb-2 mb-md-3 mb-lg-4"></app-breadcrumbs>
      <v-row>
        <router-view></router-view>
        <v-col cols="12" md="3">
          <app-forum-statistics></app-forum-statistics>
          <app-latest-threads class="mt-4"></app-latest-threads>
          <v-card flat class="mt-4">
            <v-card-title>About us</v-card-title>
            <v-card-text>
              <div class="mb-1">
                <v-icon class="material-icons-outlined mr-2" small>email</v-icon>
                support@drforummail.com
              </div>
              <div class="mb-1">
                <v-icon class="material-icons-outlined mr-2" small>phone</v-icon>
                0915212328, 0915 561 252
              </div>
              <div>
                <v-icon class="material-icons-outlined mr-2" small>payment</v-icon>
                <a href="#">Donate us</a>
              </div>
              <v-divider class="my-2"></v-divider>
              Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum
              has been the industry's standard dummy text ever since the 1500s, when an unknown
              printer
              took
            </v-card-text>
          </v-card>
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
import AppLatestThreads from '@/components/app/AppLatestThreads.vue';

export default defineComponent({
  name: 'Common',
  components: { AppLatestThreads, AppBreadcrumbs, AppForumStatistics, TheLayout },
});
</script>
