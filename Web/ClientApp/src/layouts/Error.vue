<template>
  <v-main>
    <router-view></router-view>
  </v-main>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';

export default defineComponent({
  name: 'ErrorLayout',
});
</script>
