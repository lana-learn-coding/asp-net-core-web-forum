<template>
  <v-dialog
    v-model="options.show"
    width="unset"
    @click:outside="cb(false)"
  >
    <v-card
      :width="options.width"
      :max-width="$vuetify.breakpoint.lgAndUp ? '550px' : '400px'"
      :min-width="$vuetify.breakpoint.lgAndUp ? '400px' : '300px'"
    >
      <v-card-title>{{ options.title }}</v-card-title>

      <v-card-text>{{ options.text }}</v-card-text>

      <v-card-actions>
        <v-btn color="primary" text @click="cb(true)">
          {{ options.ok }}
        </v-btn>
        <v-btn v-if="options.cancel" text @click="cb(false)">
          {{ options.cancel }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { defineComponent, reactive, watch } from '@vue/composition-api';
import { AlertOptions, useAlert } from '@/composable/message';
import { noop } from '@/composable/compat';

export default defineComponent({
  name: 'TheAppAlert',
  setup() {
    const defaultOptions: AlertOptions = {
      cb: noop as () => Promise<void>,
      ok: 'OK',
      text: 'Please confirm',
      title: 'Alert',
      cancel: 'Cancel',
      width: 'unset',
      show: false,
    };
    const options = reactive({ ...defaultOptions });
    const { dialog } = useAlert();

    watch(() => dialog.show, (val) => {
      if (!val) {
        if (options.show) cb(false);
        return;
      }
      Object.assign(options, defaultOptions);
      Object.assign(options, dialog);
    });

    async function cb(accept: boolean) {
      try {
        if (options.cb) {
          await options.cb(accept);
        }
      } finally {
        options.show = false;
        dialog.show = false;
      }
    }

    return {
      options,
      cb,
    };
  },
});
</script>
