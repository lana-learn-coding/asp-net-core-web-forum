<template>
  <v-dialog
    v-model="dialog"
    width="unset"
    @click:outside="persistent || close(false) "
    :persistent="persistent"
  >
    <v-card
      :width="width"
      :max-width="$vuetify.breakpoint.lgAndUp ? '550px' : '400px'"
      :min-width="$vuetify.breakpoint.lgAndUp ? '400px' : '300px'"
    >
      <slot :on="{ close }"></slot>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { defineComponent, ref } from '@vue/composition-api';

export default defineComponent({
  name: 'AlertDialog',
  emits: ['input', 'close'],
  props: {
    value: {
      type: Boolean,
      default: true,
    },
    width: { type: String, default: 'unset' },
    persistent: Boolean,
  },
  setup(props, { emit }) {
    // Not map computed-emit to props, as TheAppAlert component can't make the value reactive
    const dialog = ref(props.value);

    function close(accept: boolean) {
      dialog.value = false;
      emit('input', false);
      emit('close', accept);
    }

    return {
      close,
      dialog,
    };
  },
});
</script>
