<template>
  <div>
    <transition name="fade-transition" v-for="(snack, i) of snackbars" :key="snack.id">
      <v-snackbar
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
    </transition>
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
      timeout: 5000,
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
