<template>
  <v-select
    class="mb-2"
    :value="value"
    @input="x => $emit('input', x)"
    :items="sorts"
    :label="value"
    :dense="dense"
    :single-line="singleLine"
    :hide-details="hideDetails"
  >
  </v-select>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';

export default defineComponent({
  name: 'ForumSortSelect',
  props: {
    value: String,
    label: String,
    persistentPlaceholder: Boolean,
    dense: Boolean,
    singleLine: Boolean,
    hideDetails: Boolean,
  },
  setup() {
    return {
      sorts: [
        { text: 'Last Activity', value: '' },
        { text: 'Most Important', value: 'priority' },
        { text: 'Most Viewed', value: '-viewsCount' },
        { text: 'Most Threads', value: '-threadsCount' },
        { text: 'Most Posts', value: '-postsCount' },
      ],
    };
  },
});
</script>
