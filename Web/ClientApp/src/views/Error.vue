<template>
  <v-main>
    <div id="app" class="d-flex flex-column justify-center align-center">
      <div class="text--secondary text-h1">{{ code }}</div>
      <div class="text--primary mt-3">{{ text }}</div>
      <div class="mt-3">
        <v-btn class="mr-2" color="primary" text :to="{ name: 'Home' }">Home</v-btn>
        <v-btn text disabled>Report</v-btn>
      </div>
    </div>
  </v-main>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';

export default defineComponent({
  props: {
    code: String,
    text: String,
  },
  name: 'Error',
});
</script>
