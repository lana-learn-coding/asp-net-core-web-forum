<template>
  <div>Dashboard</div>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';

export default defineComponent({
  name: 'Dashboard',
  setup() {
    useTitle('Dashboard');
  },
});
</script>
