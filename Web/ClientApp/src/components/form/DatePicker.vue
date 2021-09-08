<template>
  <div>
    <v-menu
      v-model="menu"
      :close-on-content-click="false"
      transition="scale-transition"
      offset-y
      min-width="auto"
    >
      <template v-slot:activator="{ on, attrs }">
        <v-text-field
          v-model="date"
          :error-messages="errorMessages"
          :label="label || 'Date'"
          :persistent-placeholder="persistentPlaceholder"
          :required="required"
          :single-line="singleLine"
          :hide-details="hideDetails"
          :clearable="!required && !readonly"
          @click:clear="date = ''"
          :dense="dense"
          readonly
          v-bind="attrs"
          v-on="on"
        ></v-text-field>
      </template>
      <v-date-picker
        v-model="date"
        :active-picker.sync="activePicker"
        :max="(new Date(Date.now() - (new Date()).getTimezoneOffset() * 60000)).toISOString().substr(0, 10)"
        min="1950-01-01"
        @change="$refs.menu.save(date)"
        :readonly="readonly"
      ></v-date-picker>
    </v-menu>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, ref, watch } from '@vue/composition-api';
import { formatDate } from '@/composable/date';

export default defineComponent({
  name: 'DatePicker',
  props: {
    value: String,
    errorMessages: [String, Array],
    label: String,
    required: Boolean,
    persistentPlaceholder: Boolean,
    singleLine: Boolean,
    hideDetails: Boolean,
    readonly: Boolean,
    dense: Boolean,
  },
  setup(props, { emit }) {
    const date = computed({
      get: () => formatDate(props.value),
      set: (val) => {
        emit('input', val);
      },
    });
    const menu = ref(null);
    const activePicker = ref('');

    watch(menu, (val) => val && setTimeout(() => {
      activePicker.value = 'YEAR';
    }));

    return {
      menu,
      date,
      activePicker,
    };
  },
});
</script>
