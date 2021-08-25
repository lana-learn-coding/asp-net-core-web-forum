<template>
  <component :is="$route.meta.layout || 'main'">
    <router-view></router-view>
  </component>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';

export default defineComponent({
  name: 'App',
});
</script>

<style>
body, main {
  min-height: 100vh;
}
</style>
