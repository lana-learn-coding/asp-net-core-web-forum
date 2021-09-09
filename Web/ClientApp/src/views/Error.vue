<template>
  <div class="d-flex flex-column justify-center align-center" style="margin-top: 25vh">
    <div class="text--secondary text-h1">{{ code }}</div>
    <div class="text--primary mt-3">{{ text }}</div>
    <div class="mt-3">
      <v-btn class="mr-2" color="primary" text :to="{ name: 'Home' }">Home</v-btn>
      <v-btn class="mr-2" color="secondary" text @click="$router.back()">Back</v-btn>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';

export default defineComponent({
  props: {
    code: String,
    text: String,
  },
  name: 'Error',
  setup(props) {
    useTitle(`${props.code} - ${props.text}`);
  },
});
</script>
