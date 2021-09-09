<template>
  <div>
    <v-btn color="primary" @click.stop="open" text dense :disabled="disabled">
      <v-icon>add</v-icon>
      <span class="ml-1">Post</span></v-btn>
    <v-dialog v-model="dialog" width="unset" transition="fade-transition" eager>
      <v-card
        width="900px"
        :max-width="$vuetify.breakpoint.lgAndUp ? '1200px' : $vuetify.breakpoint.width - 40"
        :min-width="$vuetify.breakpoint.mdAndUp ? '500px' : '320px'"
      >
        <v-card-title>Post reply in thread</v-card-title>
        <v-card-subtitle>
          <div>Thread: {{ thread.title }}</div>
        </v-card-subtitle>
        <v-card-text>
          <v-form ref="formRef">
            <v-row>
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
import { Dictionary, Thread } from '@/services/model';
import { isAuthorized } from '@/services/auth';
import { noop, useRouter } from '@/composable/compat';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';
import EditorInput from '@/components/form/EditorInput.vue';

export default defineComponent({
  name: 'PostForm',
  components: { EditorInput, AutoCompleteSelect },
  props: {
    thread: {
      type: Object as PropType<Thread>,
      required: true,
    },
  },
  setup(props, { emit }) {
    const dialog = ref(false);
    const { form, errors, setErrors, clearErrors, clearForm } = useForm<PostForm>({
      content: '',
      threadId: props.thread?.uid,
    });
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const formRef = ref<any>(null);

    const loading = ref(false);
    const http = useHttp();
    const { notify, confirm } = useMessage();

    async function submit() {
      if (!formRef.value.validate()) {
        return;
      }
      loading.value = true;
      try {
        await http.post('posts', form);
        close();
        emit('change');
        notify({ text: 'Post created successfully', type: 'success' });
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

    const disabled = computed(() => props.thread.status !== 0);
    const router = useRouter();

    function open() {
      if (!isAuthorized('User')) {
        confirm({
          text: 'Please login to continue',
          persistent: true,
          title: 'Create Post',
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

interface PostForm extends Dictionary {
  threadId: string;
  content: string;
}
</script>
