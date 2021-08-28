<template>
  <v-row>
    <v-col
      cols="12"
      md="9"
    >
      <v-sheet rounded
               min-height="70vh">
        <!--  -->
      </v-sheet>
    </v-col>

    <v-col
      cols="12"
      md="3"
    >
      <app-forum-statistics></app-forum-statistics>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import Vue from 'vue';
import AppForumStatistics from '@/components/app/AppForumStatistics.vue';

export default Vue.extend({
  name: 'Home',
  components: { AppForumStatistics },
});
</script>
