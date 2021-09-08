<template>
  <div id="app">
    <v-navigation-drawer
      v-auth="'Admin'"
      v-model="drawer"
      app
    >
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="title">
            <app-icon></app-icon>
          </v-list-item-title>
          <v-list-item-subtitle>
            Doctors Forums
          </v-list-item-subtitle>
        </v-list-item-content>
      </v-list-item>

      <v-divider></v-divider>

      <v-list
        dense
        nav
      >
        <v-list-item-group color="primary">
          <template v-for="item in links">

            <v-list-group
              v-if="item.links"
              :key="item.title"
              :prepend-icon="item.icon"
            >
              <template v-slot:activator>
                <v-list-item-title>{{ item.title }}</v-list-item-title>
              </template>
              <v-list-item
                v-for="link in item.links"
                :key="link.title"
                :to="link.name ? { name: link.name } : null"
                exact-path
                link
              >
                <v-list-item-icon>
                  <v-icon>{{ link.icon || 'dashboard' }}</v-icon>
                </v-list-item-icon>

                <v-list-item-content>
                  <v-list-item-title>{{ link.title }}</v-list-item-title>
                </v-list-item-content>
              </v-list-item>
            </v-list-group>

            <v-list-item
              v-else
              :key="item.title"
              :to="item.name ? { name: item.name } : null"
              exact-path
              link
            >
              <v-list-item-icon>
                <v-icon>{{ item.icon || 'dashboard' }}</v-icon>
              </v-list-item-icon>

              <v-list-item-content>
                <v-list-item-title>{{ item.title }}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </template>
        </v-list-item-group>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar app color="primary" dark>
      <v-app-bar-nav-icon @click="drawer = !drawer" v-auth="'Admin'"></v-app-bar-nav-icon>
      <v-toolbar-title>{{ title }}</v-toolbar-title>
    </v-app-bar>

    <v-main>
      <v-container class="mt-3 mt-md-4 mt-lg-5 mt-xl-6">
        <router-view></router-view>
      </v-container>
    </v-main>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref, watch } from '@vue/composition-api';
import { useLocalStorage, useTitle } from '@vueuse/core';
import { useRoute, useVuetify } from '@/composable/compat';
import AppIcon from '@/components/app/AppIcon.vue';
import { useUser } from '@/services/auth';
import { useMessage } from '@/composable/message';

export default defineComponent({
  name: 'Admin',
  components: { AppIcon },
  setup() {
    const title = useTitle('Admin', { observe: true });
    const drawer = ref(useVuetify().breakpoint.lgAndUp && useUser().user.isAuthenticated);
    const links = [
      { title: 'Dashboard', icon: 'bar_chart', name: 'Dashboard' },
      {
        title: 'Topics',
        icon: 'style',
        links: [
          { title: 'Category', icon: 'label_important', name: 'ManageCategory' },
          { title: 'Tag', icon: 'local_offer', name: 'ManageTag' },
          { title: 'Specialty', icon: 'grade', name: 'ManageSpecialty' },
          { title: 'Language', icon: 'translate', name: 'ManageLanguage' },
        ],
      },
      { title: 'Forum', icon: 'folder', name: 'ManageForum' },
      { title: 'Thread', icon: 'forum', name: 'ManageThread' },
      { title: 'Post', icon: 'chat_bubble', name: 'ManagePost' },
      {
        title: 'Users',
        icon: 'person',
        links: [
          { title: 'Account', icon: 'person_outline', name: 'ManageUser' },
          { title: 'Specialty', icon: 'grade', name: 'ManageSpecialty' },
          { title: 'Country', icon: 'language', name: 'ManageCountry' },
          { title: 'City', icon: 'location_city', name: 'ManageCity' },
          { title: 'Experience', icon: 'wheelchair_pickup', name: 'ManageExperience' },
          { title: 'Position', icon: 'people_alt', name: 'ManagePosition' },
        ],
      },
      { title: 'Home', icon: 'home', name: 'Home' },
    ];

    let greeted = false;
    const greeting = useLocalStorage('drforum-greeting', true);
    const { confirm } = useMessage();
    const route = useRoute();
    const { user } = useUser();

    async function greet() {
      if (!greeting.value || greeted) return;
      if (!user.isAuthenticated || !user.roles?.includes('Admin')) return;
      if (route.name && route.name.includes('Login')) return;

      const showAgain = await confirm({
        title: 'Greeting',
        subtitle: 'Welcome back, Admin',
        text: 'Click on (?) icon to read document',
        cancel: 'Don\'t show this again',
      });
      if (!showAgain) greeting.value = false;
      greeted = true;
    }

    watch(() => route.name, greet);
    onMounted(greet);

    return {
      title,
      drawer,
      links,
    };
  },
});
</script>
