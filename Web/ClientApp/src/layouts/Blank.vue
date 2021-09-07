<template>
  <the-layout>
    <router-view></router-view>
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
