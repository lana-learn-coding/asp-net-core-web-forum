<template>
  <the-layout>
    <v-container class="mt-2 mt-md-3 mt-lg-4 mt-xl-5">
      <router-view></router-view>
    </v-container>
  </the-layout>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';
import TheLayout from '@/layouts/TheLayout.vue';

export default defineComponent({
  name: 'Blank',
  components: { TheLayout },
});
</script>
