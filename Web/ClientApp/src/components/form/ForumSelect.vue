<template>
  <v-autocomplete
    :value="value"
    @input="x => $emit('input', x)"
    :items="data"
    :loading="loading"
    :search-input.sync="search"
    :error-messages="errorMessages"
    :item-value="itemValue"
    hide-no-data
    item-text="title"
    :label="label || 'Forum'"
    :persistent-placeholder="persistentPlaceholder"
    :required="required"
    :single-line="singleLine"
    :hide-details="hideDetails"
    :clearable="!required"
    :rules="required ? [ruleRequired] : []"
    @click:clear="input('')"
  >
  </v-autocomplete>
</template>

<script lang="ts">
import { defineComponent, ref, watch } from '@vue/composition-api';
import { debounce } from 'vue-debounce';
import { useHttp } from '@/services/http';

export default defineComponent({
  name: 'ForumSelect',
  props: {
    value: String,
    itemValue: {
      type: String,
      default: 'uid',
    },
    errorMessages: [String, Array],
    label: String,
    required: Boolean,
    persistentPlaceholder: Boolean,
    singleLine: Boolean,
    hideDetails: Boolean,
  },
  setup() {
    const search = ref('');
    const data = ref([]);
    const loading = ref(false);

    function ruleRequired(val: string): boolean | string {
      return !!val.trim() || 'Please select categories';
    }

    const http = useHttp();
    watch(search, debounce(async (val) => {
      try {
        data.value = await http.get('forums/all', { params: { search: val, limit: 10 } });
      } catch (e) {
        data.value = [];
      }
    }, 350));

    return {
      data,
      loading,
      search,
      ruleRequired,
    };
  },
});
</script>
