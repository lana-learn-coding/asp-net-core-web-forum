<template>
  <v-autocomplete
      :value="value"
      @input="input"
      :items="data"
      :loading="loading"
      :search-input.sync="search"
      :error-messages="errorMessages"
      :item-value="itemValue"
      :multiple="multiple"
      hide-no-data
      :item-text="itemText"
      :label="label"
      :persistent-placeholder="persistentPlaceholder"
      :required="required"
      :single-line="singleLine"
      :hide-details="hideDetails"
      :clearable="!required"
      :dense="dense"
      :rules="required ? [ruleRequired] : []"
      @click:clear="input(multiple ? [] : '')"
  >
  </v-autocomplete>
</template>

<script lang="ts">
import { defineComponent, ref, watch } from '@vue/composition-api';
import { debounce } from 'vue-debounce';
import { useHttp } from '@/services/http';

export default defineComponent({
  name: 'AutoCompleteSelect',
  props: {
    value: [String, Array],
    uri: {
      type: String,
      required: true,
    },
    itemValue: {
      type: String,
      default: 'uid',
    },
    itemText: {
      type: String,
      default: 'text',
    },
    multiple: Boolean,
    errorMessages: [String, Array],
    label: String,
    required: Boolean,
    dense: Boolean,
    persistentPlaceholder: Boolean,
    singleLine: Boolean,
    hideDetails: Boolean,
  },
  setup(props, { emit }) {
    const search = ref('');
    const data = ref([]);
    const loading = ref(false);

    function ruleRequired(val: string | string[]): boolean | string {
      if (typeof val === 'string') {
        return !!val.trim() || 'Please select a value';
      }
      return !!val?.length || 'Please pick a value';
    }

    const http = useHttp();
    watch(search, debounce(async (val) => {
      try {
        data.value = await http.get(props.uri, { params: { search: val, limit: 10 } });
      } catch (e) {
        data.value = [];
      }
    }, 350));

    function input(val: string | string[]) {
      emit('input', val);
    }

    return {
      data,
      loading,
      search,
      ruleRequired,
      input,
    };
  },
});
</script>
