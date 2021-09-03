<template>
  <v-sheet v-if="items.length" rounded>
    <v-breadcrumbs :items="items"></v-breadcrumbs>
  </v-sheet>
</template>

<script lang="ts">
import { computed, defineComponent } from '@vue/composition-api';
import { BreadcrumbItem, breadcrumbs } from '@/composable/breadcrumbs';

export default defineComponent({
  name: 'AppBreadcrumbs',
  setup() {
    const items = computed(() => breadcrumbs.value.map((x) => ({
      to: { name: x.name },
      text: x.text,
      exact: x.extract ?? true,
      disabled: x.disabled ?? false,
    }))) as unknown as BreadcrumbItem[];

    return {
      items,
    };
  },
});
</script>
