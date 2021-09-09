<template>
  <v-dialog v-model="dialog" width="unset" transition="fade-transition" eager>
    <template #activator="{ on, attrs }">
      <v-btn color="primary" text dense v-on="on" v-bind="attrs" class="ml-2">
        <v-icon class="material-icons-outlined">filter_alt</v-icon>
        <span class="ml-1">Filter</span>
      </v-btn>
    </template>
    <v-card
      width="900px"
      :max-width="$vuetify.breakpoint.lgAndUp ? '1200px' : $vuetify.breakpoint.width - 40"
      :min-width="$vuetify.breakpoint.mdAndUp ? '500px' : '320px'"
    >
      <v-card-title>{{ type === 'post' ? 'Posts' : 'Threads' }} filter</v-card-title>
      <v-card-text>
        <v-row>
          <v-col cols="7" sm="8" md="9" lg="10">
            <v-text-field
              v-model="form.search"
              label="Keyword"
              persistent-placeholder
              dense
            >
            </v-text-field>
          </v-col>
          <v-col cols="5" sm="4" md="3" lg="2">
            <search-type-select
              v-model="type"
              label="Result type"
              persistent-placeholder
              required
              dense
            >
            </search-type-select>
          </v-col>

          <v-col cols="7" lg="9">
            <auto-complete-select
              v-model="form.forum"
              label="Forum"
              uri="forums/all"
              item-text="title"
              item-value="slug"
              persistent-placeholder
              dense
            >
            </auto-complete-select>
          </v-col>
          <v-col cols="5" sm="4" lg="3">
            <auto-complete-select
              v-model="form.tag"
              label="Tag"
              uri="tags/all"
              item-text="name"
              item-value="slug"
              persistent-placeholder
              dense
            >
            </auto-complete-select>
          </v-col>
          <v-col cols="12" v-if="type === 'post'">
            <auto-complete-select
              v-model="form.thread"
              label="Thread"
              uri="threads/all"
              item-text="title"
              item-value="slug"
              persistent-placeholder
              dense
            >
            </auto-complete-select>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-title>Authors filter</v-card-title>
      <v-card-text>
        <v-row>
          <v-col cols="12" lg="6">
            <v-text-field
              v-model="form.user"
              label="User"
              persistent-placeholder
              dense
            >
            </v-text-field>
          </v-col>

          <v-col cols="12" lg="6">
            <auto-complete-select
              label="Knowledge"
              uri="specialties/all"
              item-text="name"
              v-model="form.speciality"
              item-value="slug"
              persistent-placeholder
              dense
            >
            </auto-complete-select>
          </v-col>

          <v-col cols="6">
            <auto-complete-select
              label="City"
              uri="regions/cities/all"
              item-text="name"
              v-model="form.city"
              item-value="slug"
              persistent-placeholder
              dense
            >
            </auto-complete-select>
          </v-col>
          <v-col cols="6">
            <auto-complete-select
              label="Country"
              uri="regions/countries/all"
              item-text="name"
              v-model="form.country"
              item-value="slug"
              persistent-placeholder
              dense
            >
            </auto-complete-select>
          </v-col>

          <v-col cols="6">
            <auto-complete-select
              label="Work Position"
              uri="exp/positions/all"
              item-text="name"
              v-model="form.position"
              item-value="slug"
              persistent-placeholder
              dense
            >
            </auto-complete-select>
          </v-col>

          <v-col cols="6">
            <auto-complete-select
              label="Work Experiences"
              uri="exp/positions/all"
              item-text="name"
              v-model="form.experience"
              item-value="slug"
              persistent-placeholder
              dense
            >
            </auto-complete-select>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-actions>
        <v-btn
          class="mr-4"
          @click="submit"
          color="primary"
          text
        >
          Apply
        </v-btn>
        <v-btn
          v-if="$vuetify.breakpoint.mdAndUp"
          class="mr-4"
          @click="clear"
          color="green"
          text
        >
          Clear
        </v-btn>
        <v-btn @click="dialog = false" text>
          Back
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { computed, defineComponent, ref, watch } from '@vue/composition-api';
import { useForm } from '@/composable/form';
import { useMessage } from '@/composable/message';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';
import EditorInput from '@/components/form/EditorInput.vue';
import SearchTypeSelect from '@/components/form/SearchTypeSelect.vue';

export default defineComponent({
  name: 'AdvancedSearchForm',
  components: { SearchTypeSelect, EditorInput, AutoCompleteSelect },
  props: {
    filter: {
      type: Object,
    },
  },
  setup(props, { emit }) {
    const dialog = ref(false);
    const { form, clearForm } = useForm({
      search: '',
      type: '',
      tag: '',
      forum: '',
      experience: '',
      position: '',
      speciality: '',
      city: '',
      user: '',
      country: '',
      sort: '',
      thread: '',
    });

    const { notify } = useMessage();

    function submit() {
      emit('change', form);
      dialog.value = false;
      notify({ text: 'New filter applied' });
    }

    const type = computed<string>({
      get: () => form.type,
      set: (val) => {
        form.sort = '';
        form.thread = '';
        form.type = val;
      },
    });

    function clear() {
      clearForm();
      submit();
    }

    watch(dialog, () => {
      Object.assign(form, props.filter);
    });

    return {
      type,
      form,
      submit,
      clear,
      dialog,
    };
  },
});
</script>
