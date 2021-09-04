<template>
  <v-row class="py-2 py-xl-5">
    <v-hover #default="{ hover }">
      <v-col cols="2" md="1">
        <router-link
          v-ripple
          class="d-flex justify-center align-center w-full h-full text-decoration-none"
          :to="{ name: 'Forum', params: { slug: forum.slug } }"
        >
          <v-icon
            :small="$vuetify.breakpoint.mdOnly"
            :color="hover ? 'primary' : ''"
          >
            {{ forum.icon || 'folder' }}
          </v-icon>
        </router-link>
      </v-col>
    </v-hover>

    <v-col>
      <router-link
        class="body-1 text-truncate text-decoration-none font-weight-medium text-link"
        :to="{ name: 'Forum', params: { slug: forum.slug }, meta: { roles: checkRole(forum) } }"
      >
        {{ forum.title }}
      </router-link>
      <div class="body-2">{{ forum.subTitle }}</div>
      <div>
        <v-tooltip bottom v-if="forum.forumAccess > 0">
          <template v-slot:activator="{ on, attrs }">
            <v-icon
              small
              v-bind="attrs"
              v-on="on"
              class="material-icons-outlined"
              :color="forum.forumAccess > 1 ? 'error darken-1' : ''">
              lock
            </v-icon>
          </template>
          <span>Only member can access this forum</span>
        </v-tooltip>

        <v-tooltip bottom v-if="forum.threadAccess === 1">
          <template v-slot:activator="{ on, attrs }">
            <v-icon small v-bind="attrs" v-on="on" class="material-icons-outlined">beenhere
            </v-icon>
          </template>
          <span>Require approval when creating thread here</span>
        </v-tooltip>

        <v-tooltip bottom v-if="forum.threadAccess > 1">
          <template v-slot:activator="{ on, attrs }">
            <v-icon small v-bind="attrs" v-on="on" class="material-icons-outlined">
              admin_panel_settings
            </v-icon>
          </template>
          <span>Only admin can create a thread here</span>
        </v-tooltip>
      </div>
    </v-col>
    <v-col cols="2">
      <div class="body-2">
        <number-counter from="0" :to="forum.threadCounts || 0" :duration="1"></number-counter>
        Threads
      </div>
      <div class="body-2">
        <number-counter from="0" :to="forum.postCounts || 0" :duration="1"></number-counter>
        Posts
      </div>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { defineComponent, PropType } from '@vue/composition-api';
import NumberCounter from '@/components/NumberCounter.vue';
import { Forum } from '@/services/model';

export default defineComponent({
  name: 'ForumRow',
  components: { NumberCounter },
  props: {
    forum: Object as PropType<Forum>,
  },
  setup() {
    function checkRole(forum: Forum): string[] | boolean {
      if (forum.threadAccess < 1) {
        return false;
      }
      return forum.threadAccess === 1 ? ['User'] : ['Admin'];
    }

    return {
      checkRole,
    };
  },
});
</script>

<style scoped>

</style>
