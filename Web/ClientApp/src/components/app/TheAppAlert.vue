<template>
  <div>
    <alert-dialog
      v-for="dialog of dialogs"
      :key="dialog.id"
      width="unset"
      @close="(accept) => cb(dialog, accept)"
      :persistent="dialog.persistent"
    >
      <template #default="{ on }">
        <v-card
          :width="dialog.width"
          :max-width="$vuetify.breakpoint.lgAndUp ? '550px' : '400px'"
          :min-width="$vuetify.breakpoint.lgAndUp ? '400px' : '300px'"
        >
          <v-card-title>{{ dialog.title }}</v-card-title>
          <v-card-subtitle>{{ dialog.subtitle }}</v-card-subtitle>

          <v-card-text class="body-1">
            <div v-if="dialog.type === 'text'">{{ dialog.text }}</div>
            <v-alert v-else :type="dialog.type" class="mb-0 body-2" text>
              {{ dialog.text }}
            </v-alert>
          </v-card-text>

          <v-card-actions>
            <v-btn color="primary" text @click="on.close(true)">
              {{ dialog.ok }}
            </v-btn>
            <v-btn v-if="dialog.cancel" text @click="on.close(false)">
              {{ dialog.cancel }}
            </v-btn>
          </v-card-actions>
        </v-card>
      </template>
    </alert-dialog>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent } from '@vue/composition-api';
import { AlertOptions, alerts } from '@/composable/message';
import { useLocalId } from '@/composable/compat';
import AlertDialog from '@/components/app/AlertDialog.vue';

export default defineComponent({
  name: 'TheAppAlert',
  components: { AlertDialog },
  setup() {
    const defaultOptions: Alert = {
      cb: (bool) => bool,
      ok: 'OK',
      text: 'Please confirm',
      subtitle: '',
      title: 'Alert',
      cancel: 'Cancel',
      width: 'unset',
      type: 'info',
      persistent: false,
    };

    const dialogs = computed(() => alerts.value.map((d) => {
      const dialog = { ...defaultOptions, ...d };
      if (!dialog.id) dialog.id = useLocalId('dialog');
      (d as Alert).id = dialog.id;
      return dialog;
    })) as unknown as Alert[];

    function cb(opt: Alert, accept: boolean) {
      opt.cb(accept);
      setTimeout(() => {
        alerts.value = alerts.value.filter((a) => (a as Alert).id === opt.id);
      });
    }

    return {
      dialogs,
      cb,
    };
  },
});

interface Alert extends AlertOptions {
  id?: string;

  cb(val: boolean): void;
}
</script>
