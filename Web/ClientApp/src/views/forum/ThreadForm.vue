<template>
  <div>
    <v-btn color="primary" @click.stop="open" text dense :disabled="disabled">
      <span>New</span>
      <span class="d-none d-lg-block ml-1">Thread</span></v-btn>
    <v-dialog v-model="dialog" width="unset" transition="fade-transition" eager>
      <v-card
        width="900px"
        :max-width="$vuetify.breakpoint.lgAndUp ? '1200px' : $vuetify.breakpoint.width - 40"
        :min-width="$vuetify.breakpoint.mdAndUp ? '500px' : '320px'"
      >
        <v-card-title>Create new Thread</v-card-title>
        <v-card-subtitle>
          <div>Forum: {{ forum.title }}</div>
          <span v-if="forum.threadAccess >= 1">Note: This forum require admin review and approval before your thread can active</span>
        </v-card-subtitle>
        <v-card-text>
          <v-form ref="formRef">
            <v-row>
              <v-col cols="12">
                <v-text-field
                  v-model="form.title"
                  :error-messages="errors.title"
                  label="Title"
                  persistent-placeholder
                  required
                >
                </v-text-field>
              </v-col>

              <v-col cols="12">
                <auto-complete-select
                  label="Tags"
                  uri="tags/all"
                  v-model="form.tagIds"
                  :error-messages="errors.tagIds"
                  item-text="name"
                  item-value="uid"
                  multiple
                  persistent-placeholder
                >
                </auto-complete-select>
              </v-col>

              <v-col cols="12">
                <editor-input
                  v-model="form.content"
                  :error-messages="errors.content"
                >
                </editor-input>
              </v-col>
            </v-row>

            <div class="mt-3">
              <v-btn
                class="mr-4"
                @click="submit"
                color="primary"
                :loading="loading"
              >
                Submit
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
              <v-btn @click="close" text>
                Back
              </v-btn>
            </div>
          </v-form>
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, PropType, ref, watch } from '@vue/composition-api';
import { useHttp } from '@/services/http';
import { useForm } from '@/composable/form';
import { useMessage } from '@/composable/message';
import { AccessMode, Dictionary, Forum } from '@/services/model';
import { isAuthorized } from '@/services/auth';
import { noop } from '@/composable/compat';
import router from '@/router';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';
import EditorInput from '@/components/form/EditorInput.vue';

export default defineComponent({
  name: 'ThreadForm',
  components: { EditorInput, AutoCompleteSelect },
  props: {
    forum: {
      type: Object as PropType<Forum>,
      required: true,
    },
  },
  setup(props, { emit }) {
    const dialog = ref(false);
    const { form, errors, setErrors, clearErrors, clearForm } = useForm<ThreadForm>({
      title: '',
      content: '',
      tagIds: [],
      forumId: props.forum?.uid || '',
    });
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const formRef = ref<any>(null);

    const loading = ref(false);
    const http = useHttp();
    const { notify, confirm, alert } = useMessage();

    async function submit() {
      if (!formRef.value.validate()) {
        return;
      }
      loading.value = true;
      try {
        await http.post('threads', form);
        close();
        emit('change');
        notify({ text: 'Thread created successfully', type: 'success' });
        if (props.forum.threadAccess >= AccessMode.Protected) {
          await alert({
            title: 'Info',
            subtitle: 'Your thread have been queued',
            text: 'Thread in this forum require admin review before it can start active',
          });
        }
      } catch (e) {
        if (e.response.data) {
          setErrors(e.response.data);
          notify({ text: 'Please check the input values', type: 'warning' });
          return;
        }
      } finally {
        loading.value = false;
      }
    }

    function clear() {
      clearErrors();
      clearForm();
      formRef.value.resetValidation();
    }

    function close() {
      clear();
      dialog.value = false;
    }

    watch(dialog, clear);

    const disabled = computed(() => props.forum.threadAccess >= AccessMode.Internal && !isAuthorized('Admin'));

    function open() {
      if (!isAuthorized('User')) {
        confirm({
          text: 'Please login to continue',
          persistent: true,
          title: 'Create Thread',
        })
          .then((ok) => {
            if (ok) router.push({ name: 'Login' }).catch(noop);
          });
        return;
      }
      dialog.value = true;
    }

    return {
      disabled,
      open,
      formRef,
      form,
      errors,
      submit,
      close,
      clear,
      loading,
      dialog,
    };
  },
});

interface ThreadForm extends Dictionary {
  forumId: string;
  title: string;
  content: string;
  tagIds: string[];
}
</script>
