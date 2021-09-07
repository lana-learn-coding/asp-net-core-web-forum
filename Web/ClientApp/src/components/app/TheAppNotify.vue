<template>
  <div>
    <v-snackbar
      v-for="(snack, i) of snackbars"
      :key="snack.id"
      :style="{ 'margin-top': i * 55 + 'px'}"
      :value="true"
      :timeout="snack.timeout"
      @input="(x) => x || close(snack)"
      :color="snack.type"
      top
      right
      rounded
      text
    >
      {{ snack.text }}
    </v-snackbar>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent } from '@vue/composition-api';
import { notifies, NotifyOptions } from '@/composable/message';
import { useLocalId } from '@/composable/compat';

export default defineComponent({
  name: 'TheAppNotify',
  setup() {
    const defaultOptions: Notify = {
      text: 'Operation completed',
      timeout: 4000,
      type: 'info',
    };

    const snackbars = computed(() => notifies.value.map((n) => {
      const notify = { ...defaultOptions, ...n };
      if (!notify.id) notify.id = useLocalId('notify');
      (n as Notify).id = notify.id;
      return notify;
    })) as unknown as Notify[];

    function close(opt: Notify) {
      setTimeout(() => {
        notifies.value = notifies.value.filter((n) => (n as Notify).id !== opt.id);
      });
    }

    return {
      snackbars,
      close,
    };
  },
});

interface Notify extends NotifyOptions {
  id?: string;
}
</script>
