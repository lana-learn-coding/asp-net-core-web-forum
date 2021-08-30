<template>
  <v-sheet v-if="items.length" rounded>
    <v-breadcrumbs :items="items"></v-breadcrumbs>
  </v-sheet>
</template>

<script lang="ts">
import { computed, defineComponent } from '@vue/composition-api';
import { breadcrumbs } from '@/composable/breadcrumbs';

export default defineComponent({
  name: 'AppBreadcrumbs',
  setup() {
    const items = computed(() => breadcrumbs.items.map(x => ({
      to: { name: x.name },
      text: x.text,
      exact: x.extract ?? true,
      disabled: x.disabled ?? false,
    })));

    return {
      items,
    };
  },
});
</script>
