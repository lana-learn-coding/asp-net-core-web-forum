<template>
  <component :is="route.meta.layout || 'main'">
    <router-view></router-view>
  </component>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';
import { useRoute } from '@/composable/compat';

export default defineComponent({
  name: 'App',

  setup() {
    return {
      route: useRoute(),
    };
  },
});
</script>

<style>
body, main {
  min-height: 100vh;
}
</style>
